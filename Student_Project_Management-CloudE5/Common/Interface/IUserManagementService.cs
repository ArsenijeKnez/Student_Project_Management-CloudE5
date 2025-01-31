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
        Task<ResultMessage> RegisterAsync(LoginUser request);
        Task<ResultMessage> LoginAsync(RegisterUser request);

    }
}
