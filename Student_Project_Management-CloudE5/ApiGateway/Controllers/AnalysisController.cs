using Common.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly IAnalysisService _analysisService = ServiceProxy.Create<IAnalysisService>(new Uri("fabric:/Student_Project_Management-CloudE5/AnalysisService"));

        [HttpPost("work/{id}/analyze")]
        public async Task<IActionResult> AnalyzeWork(string id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("work/{id}/score")]
        public async Task<IActionResult> GetWorkScore(string id)
        {
            throw new NotImplementedException();
        }
    }
}
