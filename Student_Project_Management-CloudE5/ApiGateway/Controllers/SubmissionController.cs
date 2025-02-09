using ApiGateway.BlobStorage;
using Common.Dto;
using Common.Interface;
using Common.RequestForm;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {

        private readonly BlobStorageService _blobStorageService = new BlobStorageService();
        private readonly ISubmissionService _submissionService = ServiceProxy.Create<ISubmissionService>(new Uri("fabric:/Student_Project_Management-CloudE5/SubmissionService"), new ServicePartitionKey(0), TargetReplicaSelector.PrimaryReplica);

        [HttpPost("work")]
        public async Task<IActionResult> UploadWork([FromForm] SubmitNewWork work)
        {
            IFormFile file = work.file;
            if (file == null || file.Length == 0)
                return BadRequest(new ResultMessage(false, "Invalid file"));

            using var stream = file.OpenReadStream();
            var fileUrl = await _blobStorageService.UploadFileAsync(stream, file.FileName, file.ContentType);

            var result = await _submissionService.UploadWork(work.studentId, fileUrl, work.title); //Exeption serializ
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("updateWork")]
        public async Task<IActionResult> UpdateWork([FromForm] IFormFile file, [FromQuery] string studentWorkId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            using var stream = file.OpenReadStream();
            var fileUrl = await _blobStorageService.UploadFileAsync(stream, file.FileName, file.ContentType);

            var result =  await _submissionService.UpdateWork(fileUrl, studentWorkId);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("work/{studentId}/status")]
        public async Task<ActionResult<IEnumerable<StudentWorkStatus>>> GetWorkStatus(string studentId)
        {
            var statuses = await _submissionService.GetWorkStatus(studentId);
            return statuses != null ? Ok(statuses) : NotFound("No works found for the given student.");
        }

        [HttpGet("work/{studentWorkId}/feedback")]
        public async Task<ActionResult<FeedbackDto>> GetFeedback(string studentWorkId)
        {
            var feedback = await _submissionService.GetFeedback(studentWorkId);
            return feedback != null ? Ok(feedback) : NotFound("Feedback not available for this work.");
        }
    }
}
