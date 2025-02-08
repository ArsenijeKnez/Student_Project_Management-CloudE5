using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Common.Interface;
using Common.Dto;
using UserManagementService.UserDB;
using Common.Model;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Common.Mapper;
using Common.RequestForm;

namespace UserManagementService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class UserManagementService : StatefulService, IUserManagementService
    {
        private readonly UserService _userService;
        public UserManagementService(StatefulServiceContext context)
            : base(context)
        {
            _userService = new UserService("mongodb://localhost:27017", "UserDatabase", "Users");
        }

        public async Task<ResultMessage> RegisterAsync(UserDto request)
        {
            var existingUsername = await _userService.GetUserByUsernameOrEmailAsync(request.Username);
            var existingEmail = await _userService.GetUserByUsernameOrEmailAsync(request.Email);
            if (existingUsername != null || existingEmail != null)
            {
                return new ResultMessage(false,"User already exists.");
            }

            await _userService.AddUserAsync(UserMapper.ToEntity(request));

            return new ResultMessage(true, "User registered successfully." );
        }

        public async Task<UserDto> LoginAsync(LoginUser request)
        {
            var user = await _userService.GetUserByUsernameOrEmailAsync(request.UsernameOrEmail);
            if (user == null || user.PasswordHash != request.Password) 
            {
                return null;
            }

            return UserMapper.ToDto(user);
        }
        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return UserMapper.ToDto(user);
        }

        public async Task<ResultMessage> UpdateUserAsync(string id, UserDto updatedUser)
        {
            bool result = await _userService.UpdateUserAsync(id, UserMapper.ToEntity(updatedUser));
            return result? new ResultMessage(true, "User updated") : new ResultMessage(false, "Error updating user" );
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            return users.Select(u => UserMapper.ToDto(u)).ToList();
        }

        public async Task<ResultMessage> ChangeUserRoleAsync(string id, string newRole)
        {
            return await _userService.ChangeUserRoleAsync(id, newRole)? new ResultMessage (true, "Role changed" ) : new ResultMessage(false, "Error changing role" );
        }

        public async Task<ResultMessage> DeleteUserAsync(string id)
        {
            return await _userService.DeleteUserAsync(id)? new ResultMessage(true, "User deleted") : new ResultMessage(false, "Error deleting a user" );
        }


        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners() => this.CreateServiceRemotingReplicaListeners();


       
    }
}
