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
        private readonly IUserManagementService _userManagementService = ServiceProxy.Create<IUserManagementService>(new Uri("fabric:/Student_Project_Management-CloudE5/UserManagementService"), new ServicePartitionKey(0), TargetReplicaSelector.PrimaryReplica);

        [HttpPost("work")]
        public async Task<IActionResult> UploadWork([FromForm] SubmitNewWork work)
        {
            var isRestricted = await _userManagementService.IsUserRestrictedAsync("upload", work.studentId);
            if (isRestricted) return Unauthorized(new ResultMessage(false, "User submittions restricted"));

            IFormFile file = work.file;
            if (file == null || file.Length == 0)
                return BadRequest(new ResultMessage(false, "Invalid file"));

            using var stream = file.OpenReadStream();
            var fileUrl = await _blobStorageService.UploadFileAsync(stream, file.FileName, file.ContentType);

            var result = await _submissionService.UploadWork(work.studentId, fileUrl, work.title);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("updateWork")]
        public async Task<IActionResult> UpdateWork([FromForm] IFormFile file, [FromQuery] string studentWorkId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            using var stream = file.OpenReadStream();
            var fileUrl = await _blobStorageService.UploadFileAsync(stream, file.FileName, file.ContentType);

            var result =  await _submissionService.UpdateWork(fileUrl, studentWorkId);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("revertVersion")]
        public async Task<IActionResult> RevertVersion([FromQuery] string? studentWorkId, [FromQuery] int? verison)
        {
            if (studentWorkId == null || verison == null)
                return BadRequest("Invalid parameters.");


            var result = await _submissionService.RevertVersion(studentWorkId, verison.Value);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("work/{studentId}/status")]
        public async Task<ActionResult<IEnumerable<StudentWorkStatus>>> GetWorkStatus(string studentId)
        {
            var statuses = await _submissionService.GetWorkStatus(studentId);
            return statuses != null ? Ok(statuses) : NotFound("No works found for the given student.");
        }

        [HttpGet("work/{studentId}/works")]
        public async Task<ActionResult<IEnumerable<StudentWorkDto>>> GetWorksOfStudent(string studentId)
        {
            var statuses = await _submissionService.GetWorksOfStudent(studentId);
            return statuses != null ? Ok(statuses) : NotFound("No works found for the given student.");
        }

        [HttpGet("work/{studentWorkId}/feedback")]
        public async Task<ActionResult<FeedbackDto>> GetFeedback(string studentWorkId)
        {
            var feedback = await _submissionService.GetFeedback(studentWorkId);
            return feedback != null ? Ok(feedback) : NotFound("Feedback not available for this work.");
        }

        [HttpGet("work/{studentWorkId}")]
        public async Task<ActionResult<StudentWorkDto>> GetStudentWork(string studentWorkId)
        {
            var work = await _submissionService.GetStudentWork(studentWorkId);
            return work != null ? Ok(work) : NotFound("No work found for the given id.");
        }

        [HttpGet("courses")]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _submissionService.GetCourses();
            return courses != null ? Ok(courses) : NotFound(new ResultMessage(false, "No courses found"));
        }

        [HttpPost("course/new/{courseName}")]
        public async Task<IActionResult> AddCourse(string courseName)
        {
            var result = await _submissionService.AddCourse(courseName);
            return result.Success ? Ok(result) : BadRequest(new ResultMessage(false, "Failed to add course" ));
        }

        [HttpDelete("course/{courseId}")]
        public async Task<IActionResult> DeleteCourse(string courseId)
        {
            var result = await _submissionService.DeleteCourse(courseId);
            return result.Success ? Ok(result) : BadRequest(new ResultMessage(false, "Failed to delete course"));
        }

    }
}
