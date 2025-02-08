using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Dto;
using Common.Enum;
using Common.Interface;
using Common.Model;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
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
        public SubmissionService(StatefulServiceContext context)
            : base(context)
        {
            _submissionService = new StudentWorksService("mongodb://localhost:27017", "StudentWorkDatabase", "Works");
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

        public async Task<List<StudentWorkStatus>> GetWorkStatus(string studentId)
        {
            var works = await _submissionService.GetWorksByStudentIdAsync(studentId);
            return works.Select(w => new StudentWorkStatus
            {
                Title = w.Title,
                Status = w.Status,
                SubmissionDate = w.SubmissionDate,
                EstimatedAnalysisCompletion = w.EstimatedAnalysisCompletion,
            }).ToList();
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
            work.Status = WorkStatus.UnderAnalysis;

            var success = await _submissionService.UpdateWorkAsync(work.Id, work);
            return success ? new ResultMessage(true, "Work updated successfully") : new ResultMessage(false, "Failed to update work");
        }

        public async Task<ResultMessage> UploadWork(string studentId, string fileUrl, string title)
        {
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
            return new ResultMessage(true, "Work submitted successfully");
        }


        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners() => this.CreateServiceRemotingReplicaListeners();


    }
}
