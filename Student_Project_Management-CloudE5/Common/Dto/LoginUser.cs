using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class LoginUser
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
