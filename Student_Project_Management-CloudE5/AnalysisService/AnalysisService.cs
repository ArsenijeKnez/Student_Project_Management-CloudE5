using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Common.Dto;
using Common.Interface;
using Common.Model;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using System.Text.RegularExpressions;

namespace AnalysisService
{
    internal sealed class AnalysisService : StatefulService, IAnalysisService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string GEMINI_API_KEY = "AIzaSyBPBbg4TNRyJdcEehNJdF447G2WpsyiBlI";
        private const string GEMINI_API_URL = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

        public AnalysisService(StatefulServiceContext context) : base(context) { }

        public async Task<FeedbackDto> AnalyzeWork(StudentWorkDto studentWorkDto)
        {
            if (studentWorkDto == null || studentWorkDto.Versions.Count == 0)
                return new FeedbackDto { Score = 0, Errors = new List<string> { "No versions available for grading" } };

            WorkVersion versionToAnalyze = studentWorkDto.Reverted != null
                ? studentWorkDto.Versions.FirstOrDefault(v => v.VersionNumber == studentWorkDto.Reverted)
                : studentWorkDto.Versions.OrderByDescending(v => v.UploadedAt).FirstOrDefault();

            if (versionToAnalyze == null)
                return new FeedbackDto { Score = 0, Errors = new List<string> { "Invalid work version" } };

            byte[] fileData = await DownloadFileAsync(versionToAnalyze.FileUrl);
            if (fileData == null)
                return new FeedbackDto { Score = 0, Errors = new List<string> { "Failed to download file" } };

            string extractedText = Encoding.UTF8.GetString(fileData);
            return await GetAIAnalysis(extractedText);
        }

        private async Task<FeedbackDto> GetAIAnalysis(string textContent)
        {
            try
            {
                string errorPrompt = $"Identify any errors in the following work. List the errors with a brief explanation:\n{textContent}";
                string improvementPrompt = $"Suggest improvements for the following work. Provide actionable feedback:\n{textContent}";
                string scorePrompt = $"Evaluate the following work based on clarity, correctness, and structure. Provide a score between 0 and 100:\n{textContent}";


                string errors = await GetAIResponse(errorPrompt);
                string improvements = await GetAIResponse(improvementPrompt);
                string scoreResponse = await GetAIResponse(scorePrompt);

                int score = ExtractScore(scoreResponse);

                return new FeedbackDto
                {
                    Score = score,
                    Errors = errors.Split('\n').ToList(),
                    ImprovementSuggestions = improvements.Split('\n').ToList(),
                    Recommendations = new List<string>() 
                };
            }
            catch (Exception ex)
            {
                return new FeedbackDto { Score = 0, Errors = new List<string> { "AI grading failed", ex.Message } };
            }
        }

        private async Task<string> GetAIResponse(string prompt)
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };

            var requestJson = JsonSerializer.Serialize(requestBody);
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{GEMINI_API_URL}?key={GEMINI_API_KEY}", requestContent);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<GeminiResponse>(responseJson);

            return result?.candidates?.FirstOrDefault()?.content?.parts?.FirstOrDefault()?.text ?? "No response";
        }

        private int ExtractScore(string aiResponse)
        {
            if (string.IsNullOrEmpty(aiResponse))
                return 0;

            var match = Regex.Match(aiResponse, @"\b([0-9]{1,3})\b");
            if (match.Success && int.TryParse(match.Value, out int score))
            {
                return Math.Clamp(score, 0, 100);
            }

            return 50; 
        }

        private async Task<byte[]> DownloadFileAsync(string fileUrl)
        {
            try
            {
                return await _httpClient.GetByteArrayAsync(fileUrl);
            }
            catch
            {
                return null;
            }
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners() => this.CreateServiceRemotingReplicaListeners();
    }

    public class GeminiResponse
    {
        public List<Candidate> candidates { get; set; }
    }

    public class Candidate
    {
        public Content content { get; set; }
    }

    public class Content
    {
        public List<Part> parts { get; set; }
    }

    public class Part
    {
        public string text { get; set; }
    }
}




//using System;
//using System.Collections.Generic;
//using System.Fabric;
//using System.Linq;
//using System.Net.Http;
//using System.Text.Json;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Common.Dto;
//using Common.Interface;
//using Common.Model;
//using Microsoft.ServiceFabric.Data.Collections;
//using Microsoft.ServiceFabric.Services.Communication.Runtime;
//using Microsoft.ServiceFabric.Services.Remoting.Runtime;
//using Microsoft.ServiceFabric.Services.Runtime;


//namespace AnalysisService
//{
//    /// <summary>
//    /// An instance of this class is created for each service instance by the Service Fabric runtime.
//    /// </summary>
//    internal sealed class AnalysisService : StatefulService, IAnalysisService
//    {
//        private readonly HttpClient _httpClient = new HttpClient();
//        public AnalysisService(StatefulServiceContext context)
//            : base(context)
//        { }
//        public async Task<FeedbackDto> AnalyzeWork(StudentWorkDto studentWorkDto)
//        {
//            if (studentWorkDto == null || studentWorkDto.Versions.Count == 0)
//                return new FeedbackDto { Score = 0, Errors = new List<string> { "No versions available for grading" } };

//            WorkVersion versionToAnalyze = studentWorkDto.Reverted != null
//                ? studentWorkDto.Versions.FirstOrDefault(v => v.VersionNumber == studentWorkDto.Reverted)
//                : studentWorkDto.Versions.OrderByDescending(v => v.UploadedAt).FirstOrDefault();

//            if (versionToAnalyze == null)
//                return new FeedbackDto { Score = 0, Errors = new List<string> { "Invalid work version" } };

//            byte[] fileData = await DownloadFileAsync(versionToAnalyze.FileUrl);
//            if (fileData == null)
//                return new FeedbackDto { Score = 0, Errors = new List<string> { "Failed to download file" } };

//            FeedbackDto feedback = new FeedbackDto
//            {
//                Score = new Random().NextDouble() * 100, 
//                Errors = new List<string>(),
//                ImprovementSuggestions = new List<string> { "Improve structure", "Clarify main points" },
//                Recommendations = new List<string>()
//            };
//            return feedback ?? new FeedbackDto { Score = 0, Errors = new List<string> { "AI grading failed" } };
//        }

//        private async Task<byte[]> DownloadFileAsync(string fileUrl)
//        {
//            try
//            {
//                return await _httpClient.GetByteArrayAsync(fileUrl);
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners() => this.CreateServiceRemotingReplicaListeners();

//    }
//}

