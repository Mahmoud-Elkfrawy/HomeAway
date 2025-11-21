using HomeAway.Application.DTOs;
using HomeAway.Application.Interfaces;
using HomeAway.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CreateUserAsync(UserDto userDto)
        {
            var user = new Infrastructure.Identity.ApplicationUser
            {
                FullName = userDto.FullName,
                UserName = userDto.UserName,
                Email = userDto.Email
            };

            await _userManager.CreateAsync(user, userDto.Password);
            return true;
        }

        public async Task<UserDto> GetUserByIdAsync(String id)
        {
            var user = await _userManager.FindByIdAsync(id);

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
