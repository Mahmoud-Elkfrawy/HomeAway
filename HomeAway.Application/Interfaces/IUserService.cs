using HomeAway.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(String id);
        Task<bool> CreateUserAsync(UserDto userDto);
    }
}
