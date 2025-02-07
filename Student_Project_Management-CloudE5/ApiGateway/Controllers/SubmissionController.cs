using ApiGateway.BlobStorage;
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
    public class SubmissionController : ControllerBase
    {

        private readonly BlobStorageService _blobStorageService = new BlobStorageService();
        private readonly ISubmissionService _submissionService = ServiceProxy.Create<ISubmissionService>(new Uri("fabric:/Student_Project_Management-CloudE5/SubmissionService"), new ServicePartitionKey(0), TargetReplicaSelector.PrimaryReplica);

        [HttpPost("work")]
        public async Task<IActionResult> UploadWork([FromForm] IFormFile file, [FromQuery] string studentId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            using var stream = file.OpenReadStream();
            var fileUrl = await _blobStorageService.UploadFileAsync(stream, file.FileName, file.ContentType);

            //await _submissionService.SubmitWorkAsync(studentId, fileUrl, file.FileName);

            return Ok(new { FileUrl = fileUrl });
        }

        [HttpGet("work/{id}/status")]
        public async Task<IActionResult> GetWorkStatus(string id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("work/{id}/feedback")]
        public async Task<IActionResult> GetFeedback(string id)
        {
            throw new NotImplementedException();
        }
    }
}
