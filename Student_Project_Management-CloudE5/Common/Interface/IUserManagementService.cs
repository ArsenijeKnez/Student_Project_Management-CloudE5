using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
using Common.RequestForm;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Common.Interface
{
    public interface IUserManagementService : IService
    {
        Task<ResultMessage> RegisterAsync(UserDto request);
        Task<UserDto> LoginAsync(LoginUser request);
        Task<UserDto> GetUserByIdAsync(string id);
        Task<ResultMessage> UpdateUserAsync(string id, UserDto request);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<List<UserDto>> GetAllStudentsAsync();
        Task<ResultMessage> ChangeUserRoleAsync(string id, string newRole);
        Task<ResultMessage> DeleteUserAsync(string id);
        Task<ResultMessage> AddUserRestrictionAsync(string restrictionKey, string userId);
        Task<ResultMessage> RemoveUserRestrictionAsync(string restrictionKey, string userId);
        Task<bool> IsUserRestrictedAsync(string restrictionKey, string userId);
        Task<List<string>> GetUserRestrictions(string userId); 

    }
}
