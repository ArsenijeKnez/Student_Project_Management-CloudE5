using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Dto;
using Common.Interface;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ProgressService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class ProgressService : StatelessService, IProgressService
    {
        public ProgressService(StatelessServiceContext context)
            : base(context)
        { }

        private string FindMostCommonMistake(List<StudentWorkDto> studentWorks)
        {
            var allErrors = studentWorks
                .Where(w => w.Feedback != null && w.Feedback.Errors != null)
                .SelectMany(w => w.Feedback.Errors)
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Select(e => e.Trim().ToLower()) // Normalize errors
                .ToList();

            if (allErrors.Count == 0)
            {
                return "No mistakes found.";
            }

            var groupedErrors = allErrors
                .GroupBy(e => e)
                .OrderByDescending(g => g.Count())
                .ToList();

            if (groupedErrors.Count < 2 || groupedErrors.First().Count() == 1)
            {
                return "No common mistakes";
            }

            return groupedErrors.First().Key;
        }


        public Task<ClassProgress> GenerateClassProgress(List<StudentWorkDto> studentWorks)
        {
            if (studentWorks == null || studentWorks.Count == 0)
            {
                return Task.FromResult(new ClassProgress
                {
                    TotalStudents = 0,
                    AverageClassScore = 0,
                    StudentProgressList = new List<StudentProgress>(),
                    CommonMistake = "No mistakes found."
                });
            }

            var studentGroups = studentWorks
                .Where(w => w.Feedback != null)
                .GroupBy(w => w.StudentId)
                .Select(group => GenerateStudentProgress(group.Key, group.ToList()).Result)
                .ToList();

            double averageClassScore = studentGroups.Count > 0
                ? studentGroups.Average(sp => sp.AverageScore)
                : 0;

            var commonMistake = FindMostCommonMistake(studentWorks);

            return Task.FromResult(new ClassProgress
            {
                TotalStudents = studentGroups.Count,
                AverageClassScore = Math.Round(averageClassScore, 2),
                StudentProgressList = studentGroups,
                CommonMistake = commonMistake
            });
        }


        public async Task<StudentProgress> GenerateStudentProgress(string studentId, List<StudentWorkDto> studentWorks)
        {
            var relevantWorks = studentWorks.Where(w => w.StudentId == studentId && w.Feedback != null).ToList();

            int totalWorks = relevantWorks.Count;
            double averageScore = totalWorks > 0
                ? relevantWorks.Average(w => w.Feedback.Score)
                : 0;

            var scoreHistory = relevantWorks
                .OrderBy(w => w.SubmissionDate)
                .ToDictionary(w => w.SubmissionDate, w => w.Feedback.Score);

            return new StudentProgress
            {
                StudentId = studentId,
                TotalWorks = totalWorks,
                AverageScore = Math.Round(averageScore, 2),
                ScoreHistory = scoreHistory
            };
        }


        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners() => this.CreateServiceRemotingInstanceListeners();
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
