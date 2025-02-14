using Common.Dto;
using Common.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using ZstdSharp.Unsafe;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ISubmissionService _submissionService = ServiceProxy.Create<ISubmissionService>(new Uri("fabric:/Student_Project_Management-CloudE5/SubmissionService"), new ServicePartitionKey(0), TargetReplicaSelector.PrimaryReplica);
        private readonly IProgressService _progressService = ServiceProxy.Create<IProgressService>(new Uri("fabric:/Student_Project_Management-CloudE5/ProgressService"));

        [HttpGet("progress/{studentId}")]
        public async Task<ActionResult<StudentProgress>> GetStudentProgress(string studentId)
        {
            List<StudentWorkDto> works = await _submissionService.GetWorksOfStudent(studentId);
            if (works == null || works.Count == 0) return NotFound();
            StudentProgress progress = await _progressService.GenerateStudentProgress(studentId, works);
            return Ok(progress);
        }
    }
}

