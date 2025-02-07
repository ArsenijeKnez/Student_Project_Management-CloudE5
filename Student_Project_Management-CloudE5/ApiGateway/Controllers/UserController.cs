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
    public class UserController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService = ServiceProxy.Create<IUserManagementService>(new Uri("fabric:/Student_Project_Management-CloudE5/UserManagementService"), new ServicePartitionKey(0), TargetReplicaSelector.PrimaryReplica);

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(string id)
        {
            var user = await _userManagementService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserDto updatedUser)
        {
            var result = await _userManagementService.UpdateUserAsync(id, updatedUser);
            if (!result.Success) return BadRequest(result.Message);
            return NoContent();
        }

        [HttpGet("admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userManagementService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPut("admin/{id}/role")]
        public async Task<IActionResult> ChangeUserRole(string id, [FromBody] string newRole)
        {
            var result = await _userManagementService.ChangeUserRoleAsync(id, newRole);
            if (!result.Success) return BadRequest(result.Message);
            return NoContent();
        }

        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userManagementService.DeleteUserAsync(id);
            if (!result.Success) return BadRequest(result.Message);
            return NoContent();
        }
    }
}
