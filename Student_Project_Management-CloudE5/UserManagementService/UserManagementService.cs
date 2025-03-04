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

        private async Task<IReliableDictionary<string, List<string>>> GetRestrictionsDictionary()
        {
            return await this.StateManager.GetOrAddAsync<IReliableDictionary<string, List<string>>>("UserRestrictions");
        }

        public async Task<ResultMessage> AddUserRestrictionAsync(string restrictionKey, string userId)
        {
            var restrictions = await GetRestrictionsDictionary();

            using (var tx = this.StateManager.CreateTransaction())
            {
                var existingEntry = await restrictions.TryGetValueAsync(tx, restrictionKey);
                List<string> userIds = existingEntry.HasValue ? existingEntry.Value : new List<string>();

                if (!userIds.Contains(userId))
                {
                    userIds.Add(userId);
                    await restrictions.SetAsync(tx, restrictionKey, userIds);
                    await tx.CommitAsync();
                    return new ResultMessage(true, $"User {userId} added to restriction {restrictionKey}.");
                }

                return new ResultMessage(false, "User is already restricted.");
            }
        }


        public async Task<ResultMessage> RemoveUserRestrictionAsync(string restrictionKey, string userId)
        {
            var restrictions = await GetRestrictionsDictionary();

            using (var tx = this.StateManager.CreateTransaction())
            {
                var existingEntry = await restrictions.TryGetValueAsync(tx, restrictionKey);
                if (!existingEntry.HasValue) return new ResultMessage(false, "Restriction does not exist.");

                List<string> userIds = existingEntry.Value;
                if (userIds.Remove(userId))
                {
                    await restrictions.SetAsync(tx, restrictionKey, userIds);
                    await tx.CommitAsync();
                    return new ResultMessage(true, $"User {userId} removed from restriction {restrictionKey}.");
                }

                return new ResultMessage(false, "User is not in the restriction list.");
            }
        }

        public async Task<bool> IsUserRestrictedAsync(string restrictionKey, string userId)
        {
            var restrictions = await GetRestrictionsDictionary();

            using (var tx = this.StateManager.CreateTransaction())
            {
                var existingEntry = await restrictions.TryGetValueAsync(tx, restrictionKey);
                return existingEntry.HasValue && existingEntry.Value.Contains(userId);
            }
        }

        public async Task<List<string>> GetUserRestrictions(string userId)
        {
            var restrictions = await GetRestrictionsDictionary();
            List<string> userRestrictions = new List<string>();

            using (var tx = this.StateManager.CreateTransaction())
            {
                var enumerator = (await restrictions.CreateEnumerableAsync(tx)).GetAsyncEnumerator();

                while (await enumerator.MoveNextAsync(CancellationToken.None))
                {
                    var restrictionKey = enumerator.Current.Key;
                    var userIds = enumerator.Current.Value;

                    if (userIds.Contains(userId))
                    {
                        userRestrictions.Add(restrictionKey);
                    }
                }
            }

            return userRestrictions;
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
        public async Task<List<UserDto>> GetAllStudentsAsync()
        {
            var users = await _userService.GetAllStudentsAsync();
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
