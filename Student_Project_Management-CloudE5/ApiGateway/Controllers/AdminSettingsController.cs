using Common.Dto;
using Common.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminSettingsController : ControllerBase
    {
        private readonly ISubmissionService _submissionService = ServiceProxy.Create<ISubmissionService>(new Uri("fabric:/Student_Project_Management-CloudE5/SubmissionService"), new ServicePartitionKey(0), TargetReplicaSelector.PrimaryReplica);
        private readonly IAnalysisService _analysisService = ServiceProxy.Create<IAnalysisService>(new Uri("fabric:/Student_Project_Management-CloudE5/AnalysisService"));

        [HttpPost("setDailySubmissionLimit")]
        public async Task<IActionResult> SetDailySubmissionLimit([FromQuery] int limit)
        {
            if (limit <= 0)
                return BadRequest("Submission limit must be greater than zero.");

            var result = await _submissionService.SetDailySubmissionLimit(limit);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("setAnalysisInterval")]
        public async Task<IActionResult> SetAnalysisInterval([FromQuery] TimeSpan interval)
        {
            var result = await _submissionService.SetProcessingInterval(interval);
            return result != null? Ok(result) : BadRequest(result);

        }

        [HttpPut("setAnalysisMethods")]
        public async Task<IActionResult> SetAnalysisMethods([FromBody] SetPromptsDto promptsDto)
        {
            if (promptsDto == null)
                return BadRequest(new ResultMessage(false, "No prompts given"));

            var result = await _analysisService.SetPrompts(promptsDto.ErrorPrompt, promptsDto.ImprovementPrompt, promptsDto.ScorePrompt);
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpGet("getAnalysisMethods")]
        public async Task<IActionResult> GetAnalysisMethods()
        {
            var prompts = await _analysisService.GetPrompts();
            return prompts != null ? Ok(prompts) : NotFound("No prompts found.");
        }


    }

}
