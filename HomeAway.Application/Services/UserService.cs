using HomeAway.Application.Auth;
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

        public async Task<bool> CreateUserAsync(RegisterDto userDto)
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
        public async Task<bool> AssignRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, role);
                return true;
            }
            return false;

        }

        public async Task<bool> UpdateUserAsync(UserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id);

            if (user != null)
            {
                user.FullName = userDto.FullName;
                user.UserName = userDto.UserName;
                user.Email = userDto.Email;
                await _userManager.UpdateAsync(user);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return true;
            }
            return false;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = new List<UserDto>();
            List<ApplicationUser> users1 = _userManager.Users.ToList();
            foreach (var user in users1)
            {
                var role = await _userManager.GetRolesAsync(user); // <-- get roles

                users.Add(new UserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = role.ToList()

                });
            }
            return users;
        }
    }

}
