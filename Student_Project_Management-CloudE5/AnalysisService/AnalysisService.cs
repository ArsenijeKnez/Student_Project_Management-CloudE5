using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Common.Dto;
using Common.Interface;
using Common.Model;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;


namespace AnalysisService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class AnalysisService : StatelessService, IAnalysisService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public AnalysisService(StatelessServiceContext context)
            : base(context)
        { }
        public async Task<FeedbackDto> AnalyzeWork(StudentWorkDto studentWorkDto)
        {
            if (studentWorkDto == null || studentWorkDto.Versions.Count == 0)
                return new FeedbackDto { Score = 0, Errors = new List<string> { "No versions available for grading" } };

            // Determine which version to analyze
            WorkVersion versionToAnalyze = studentWorkDto.Reverted != null
                ? studentWorkDto.Versions.FirstOrDefault(v => v.VersionNumber == studentWorkDto.Reverted)
                : studentWorkDto.Versions.OrderByDescending(v => v.UploadedAt).FirstOrDefault();

            if (versionToAnalyze == null)
                return new FeedbackDto { Score = 0, Errors = new List<string> { "Invalid work version" } };

            byte[] fileData = await DownloadFileAsync(versionToAnalyze.FileUrl);
            if (fileData == null)
                return new FeedbackDto { Score = 0, Errors = new List<string> { "Failed to download file" } };

            FeedbackDto feedback = new FeedbackDto
            {
                Score = new Random().NextDouble() * 100, 
                Errors = new List<string>(),
                ImprovementSuggestions = new List<string> { "Improve structure", "Clarify main points" }
            };
            return feedback ?? new FeedbackDto { Score = 0, Errors = new List<string> { "AI grading failed" } };
        }

        private async Task<byte[]> DownloadFileAsync(string fileUrl)
        {
            try
            {
                return await _httpClient.GetByteArrayAsync(fileUrl);
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.ServiceMessage(this.Context, "File download failed: {0}", ex.Message);
                return null;
            }
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
