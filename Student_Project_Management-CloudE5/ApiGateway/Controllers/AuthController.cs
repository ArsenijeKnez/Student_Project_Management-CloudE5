﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.ServiceFabric.Services.Client;
using Common.Interface;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Common.Dto;
using Common.RequestForm;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserManagementService _userService = ServiceProxy.Create<IUserManagementService>(new Uri("fabric:/Student_Project_Management-CloudE5/UserManagementService"), new ServicePartitionKey(0), TargetReplicaSelector.PrimaryReplica);


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] UserDto request)
        {
            var result = await _userService.RegisterAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginUser request)
        {
            var user = await _userService.LoginAsync(request);
            if(user == null)return  Unauthorized(new ResultMessage(false, "Invalid credentials"));

            var isRestricted = await _userService.IsUserRestrictedAsync("login", user.Id);
            return isRestricted ? Unauthorized(new ResultMessage(false, "User login is restricted")) : Ok(user);
        }

    }
}
