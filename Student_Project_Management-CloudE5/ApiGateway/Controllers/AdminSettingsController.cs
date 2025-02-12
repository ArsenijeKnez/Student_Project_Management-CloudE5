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


        [HttpPost("setDailySubmissionLimit")]
        public async Task<IActionResult> SetDailySubmissionLimit([FromQuery] int limit)
        {
            if (limit <= 0)
                return BadRequest("Submission limit must be greater than zero.");

            var result = await _submissionService.SetDailySubmissionLimit(limit);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }

}
