using HomeAway.Application.Auth;
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
        Task<bool> CreateUserAsync(RegisterDto userDto);
        Task<bool> UpdateUserAsync(UserDto userDto);
        Task<bool> DeleteUserAsync(String id);
        Task<List<UserDto>> GetAllUsersAsync();

    }
}
