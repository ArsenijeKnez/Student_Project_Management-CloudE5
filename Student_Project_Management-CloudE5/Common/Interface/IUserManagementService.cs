using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
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
        Task<ResultMessage> ChangeUserRoleAsync(string id, string newRole);
        Task<ResultMessage> DeleteUserAsync(string id);

    }
}
