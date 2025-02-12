using Common.Dto;
using Common.Interface;
using Common.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly IAnalysisService _analysisService = ServiceProxy.Create<IAnalysisService>(new Uri("fabric:/Student_Project_Management-CloudE5/AnalysisService"));
        private readonly ISubmissionService _submissionService = ServiceProxy.Create<ISubmissionService>(new Uri("fabric:/Student_Project_Management-CloudE5/SubmissionService"), new ServicePartitionKey(0), TargetReplicaSelector.PrimaryReplica);

        [HttpPost("/{studentWorkId}")]
        public async Task<IActionResult> AnalyzeWork(string studentWorkId)
        {
            StudentWorkDto work = await _submissionService.GetStudentWork(studentWorkId);
            if( work == null ) 
                return NotFound("No work found for the given id.");
            FeedbackDto feedback = await _analysisService.AnalyzeWork(work);
            if (feedback == null)
                return BadRequest("Error generating analysis.");
            else return Ok(feedback);
        }

        [HttpPut("/append")]
        public async Task<IActionResult> ProfessorsWorkAnalysis([FromForm] FeedbackDto feedback, [FromQuery] string studentWorkId)
        {

            var result = await _submissionService.UpdateFeedback(feedback, studentWorkId);
            if (result == null)
                return BadRequest("Error updating analysis.");
            else return Ok();
        }

    }
}
