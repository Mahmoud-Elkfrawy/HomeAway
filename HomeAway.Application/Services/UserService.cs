using HomeAway.Application.DTOs;
using HomeAway.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> CreateUserAsync(UserDto userDto)
        {
            var user = new Infrastructure.Identity.ApplicationUser
            {
                FullName = userDto.FullName,
                UserName = userDto.UserName,
                Email = userDto.Email
            };

            await _userRepository.AddAsync(user);
            return true;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }
}
