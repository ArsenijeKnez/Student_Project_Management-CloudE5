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
