using Common.Dto;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mapper
{
    public static class UserMapper
    {
        public static UserDto ToDto(User user)
        {
            if (user == null) return null;

            return new UserDto
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.PasswordHash, 
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role
            };
        }

        public static User ToEntity(UserDto userDto)
        {
            if (userDto == null) return null;

            return new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = userDto.Password,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Role = userDto.Role
            };
        }
    }

}
