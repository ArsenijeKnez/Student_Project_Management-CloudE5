using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Common.Dto;
using Common.Enum;
using Common.Interface;
using Common.Mapper;
using Common.Model;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using MongoDB.Bson;
using SubmissionService.SubmissionDB;

namespace SubmissionService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class SubmissionService : StatefulService, ISubmissionService
    {
        private readonly StudentWorksService _submissionService;
        private readonly CourseService _courseService;

        private readonly IReliableStateManager _stateManager;
        private readonly IAnalysisService _analysisService = ServiceProxy.Create<IAnalysisService>(new Uri("fabric:/Student_Project_Management-CloudE5/AnalysisService"), new ServicePartitionKey(0), TargetReplicaSelector.PrimaryReplica);
        
        private const string SubmissionConfigKey = "DailySubmissionLimit";
        private const string DailySubmissionsKey = "DailySubmissions";

        private TimeSpan _processingInterval = TimeSpan.FromMinutes(5);

        public SubmissionService(StatefulServiceContext context)
            : base(context)
        {
            _submissionService = new StudentWorksService("mongodb://localhost:27017", "StudentWorkDatabase", "Works");
            _courseService = new CourseService("mongodb://localhost:27017", "StudentWorkDatabase", "Courses");
            _stateManager = this.StateManager;

        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            await LoadProcessingInterval();
            while (!cancellationToken.IsCancellationRequested)
            {
                await ProcessSubmittedWorks();
                await Task.Delay(_processingInterval, cancellationToken);
            }
        }

        private async Task LoadProcessingInterval()
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var configDict = await _stateManager.GetOrAddAsync<IReliableDictionary<string, TimeSpan>>("ProcessingConfig");
                var result = await configDict.TryGetValueAsync(tx, "ProcessingInterval");

                if (result.HasValue)
                {
                    _processingInterval = result.Value;
                }
            }
        }

        private async Task ProcessSubmittedWorks()
        {
            var submittedWorks = await _submissionService.GetWorksByStatusAsync(WorkStatus.Submitted);

            foreach (var work in submittedWorks)
            {
                var feedbackDto = await _analysisService.AnalyzeWork(StudentWorkMapper.ToDto(work));
                if (feedbackDto != null)
                {
                    work.Feedback = new Feedback
                    {
                        Score = feedbackDto.Score,
                        Errors = feedbackDto.Errors,
                        ImprovementSuggestions = feedbackDto.ImprovementSuggestions,
                        Recommendations = feedbackDto.Recommendations
                    };
                    work.Status = WorkStatus.FeedbackReady;
                    await _submissionService.UpdateWorkAsync(work.Id, work);
                }
            }
        }

        public async Task<ResultMessage> SetProcessingInterval(TimeSpan interval)
        {

            using (var tx = _stateManager.CreateTransaction())
            {
                var configDict = await _stateManager.GetOrAddAsync<IReliableDictionary<string, TimeSpan>>("ProcessingConfig");
                await configDict.SetAsync(tx, "ProcessingInterval", interval);
                await tx.CommitAsync();
            }

            _processingInterval = interval;
            return new ResultMessage(true, "Successfully set analysis interval");
        }


        public async Task<FeedbackDto> GetFeedback(string studentWorkId)
        {
            var work = await _submissionService.GetWorkByIdAsync(studentWorkId);
            if (work == null || work.Feedback == null) return null;

            return new FeedbackDto
            {
                Score = work.Feedback.Score,
                Errors = work.Feedback.Errors,
                ImprovementSuggestions = work.Feedback.ImprovementSuggestions,
                Recommendations = work.Feedback.Recommendations
            };
        }

        public async Task<StudentWorkDto> GetStudentWork(string studentWorkId)
        {
            var work = await _submissionService.GetWorkByIdAsync(studentWorkId);
            return work == null ? null : StudentWorkMapper.ToDto(work);
        }

        public async Task<List<StudentWorkDto>> GetAllStudentWorks()
        {
            var works = await _submissionService.GetAllWorksAsync();
            List<StudentWorkDto> workDtos = new List<StudentWorkDto>();
            if(works == null || works.Count == 0)
            {
                return new List<StudentWorkDto>();
            }
            else
            {
                foreach(var work in works) 
                    workDtos.Add(StudentWorkMapper.ToDto(work));
                return workDtos;
            }
        }

        public async Task<List<StudentWorkStatus>> GetWorkStatus(string studentId)
        {
            var works = await _submissionService.GetWorksByStudentIdAsync(studentId);
            return works.Select(w => new StudentWorkStatus
            {
                WorkId = w.Id,
                Title = w.Title,
                Status = w.Status,
                SubmissionDate = w.SubmissionDate,
                EstimatedAnalysisCompletion = w.EstimatedAnalysisCompletion,
            }).ToList();
        }

        public async Task<List<StudentWorkDto>> GetWorksOfStudent(string studentId)
        {
            var works = await _submissionService.GetWorksByStudentIdAsync(studentId);
            if (works == null || !works.Any())
                return null;
            List<StudentWorkDto> list = new List<StudentWorkDto>();
            foreach(var work in works)
            {
                list.Add(StudentWorkMapper.ToDto(work));
            }
            return list;
        }

        public async Task<ResultMessage> RevertVersion(string studentWorkId, int version)
        {
            var work = await _submissionService.GetWorkByIdAsync(studentWorkId);
            if (work == null) return new ResultMessage(false, "Work not found");

            work.Reverted = (uint)version;

            var success = await _submissionService.UpdateWorkAsync(work.Id, work);
            return success ? new ResultMessage(true, "Successfully reverted") : new ResultMessage(false, "Failed to reverte work");
        }

        public async Task<ResultMessage> UpdateWork(string fileUrl, string studentWorkId)
        {
            var work = await _submissionService.GetWorkByIdAsync(studentWorkId);
            if (work == null) return new ResultMessage(false, "Work not found");

            var newVersion = new WorkVersion
            {
                VersionNumber = work.Versions.Count + 1,
                FileUrl = fileUrl,
                UploadedAt = DateTime.UtcNow
            };

            work.Versions.Add(newVersion);

            var success = await _submissionService.UpdateWorkAsync(work.Id, work);
            return success ? new ResultMessage(true, "Work updated successfully") : new ResultMessage(false, "Failed to update work");
        }

        public async Task<ResultMessage> UploadWork(string studentId, string fileUrl, string title)
        {
            var today = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");

            using (var tx = _stateManager.CreateTransaction())
            {
                var dailySubmissions = await _stateManager.GetOrAddAsync<IReliableDictionary<string, int>>(DailySubmissionsKey);
                var configDict = await _stateManager.GetOrAddAsync<IReliableDictionary<string, int>>(SubmissionConfigKey);

                var limitResult = await configDict.TryGetValueAsync(tx, "MaxSubmissionsPerDay");
                int maxSubmissions = limitResult.HasValue ? limitResult.Value : 5;

                var studentKey = $"{studentId}_{today}";
                var submissionCount = await dailySubmissions.TryGetValueAsync(tx, studentKey);
                int currentCount = submissionCount.HasValue ? submissionCount.Value : 0;

                if (currentCount >= maxSubmissions)
                {
                    return new ResultMessage(false, "Daily submission limit reached.");
                }

                var newWork = new StudentWork
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    StudentId = studentId,
                    Title = title,
                    Versions = new List<WorkVersion>
                    {
                        new WorkVersion
                        {
                            VersionNumber = 1,
                            FileUrl = fileUrl,
                            UploadedAt = DateTime.UtcNow
                        }
                    },
                    Status = WorkStatus.Submitted,
                    SubmissionDate = DateTime.UtcNow,
                    EstimatedAnalysisCompletion = null,
                    Feedback = null
                };

                await _submissionService.AddWorkAsync(newWork);

                await dailySubmissions.SetAsync(tx, studentKey, currentCount + 1);
                await tx.CommitAsync();

                return new ResultMessage(true, "Work submitted successfully.");
            }
        }

        public async Task<ResultMessage> SetDailySubmissionLimit(int limit)
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var configDict = await _stateManager.GetOrAddAsync<IReliableDictionary<string, int>>(SubmissionConfigKey);
                await configDict.SetAsync(tx, "MaxSubmissionsPerDay", limit);
                await tx.CommitAsync();
            }
            return new ResultMessage(true, "Daily submission limit updated.");
        }

        public async Task<int> GetDailySubmissionLimit()
        {
            using (var tx = _stateManager.CreateTransaction())
            {
                var configDict = await _stateManager.GetOrAddAsync<IReliableDictionary<string, int>>(SubmissionConfigKey);
                var result = await configDict.TryGetValueAsync(tx, "MaxSubmissionsPerDay");
                return result.HasValue ? result.Value : 5; 
            }
        }

        public async Task<ResultMessage> UpdateFeedback(FeedbackDto feedbackDto, string studentWorkId)
        {
            var work = await _submissionService.GetWorkByIdAsync(studentWorkId);
            if (work == null) return new ResultMessage(false, "Work not found");


            work.Feedback = new Feedback
            {
                Score = feedbackDto.Score,
                Errors = feedbackDto.Errors,
                ImprovementSuggestions = feedbackDto.ImprovementSuggestions,
                Recommendations = feedbackDto.Recommendations
            };

            var success = await _submissionService.UpdateWorkAsync(work.Id, work);
            return success ? new ResultMessage(true, "Feedback updated successfully") : new ResultMessage(false, "Failed to update feedback");
        }


        public async Task<List<Course>> GetCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return courses;
        }

        public async Task<ResultMessage> AddCourse(string name)
        {
            var course = new Course { Id = ObjectId.GenerateNewId().ToString(), Name = name };
            await _courseService.AddCourseAsync(course);
            return new ResultMessage(true, "Course added successfully");
        }

        public async Task<ResultMessage> DeleteCourse(string courseId)
        {
            var deleted = await _courseService.DeleteCourseAsync(courseId);
            return deleted
                ? new ResultMessage(true, "Course deleted successfully")
                : new ResultMessage(false, "Course not found");
        }


        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners() => this.CreateServiceRemotingReplicaListeners();

    }
}
