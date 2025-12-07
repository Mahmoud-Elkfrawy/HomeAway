using HomeAway.Application.Auth;
using HomeAway.Application.DTOs;
using HomeAway.Application.Services;
using HomeAway.Application.Interfaces;
using HomeAway.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HomeAway.Test
{
    public class UserServiceTests
    {
        private UserService GetService(UserManager<ApplicationUser> userManager)
        {
            return new UserService(userManager);
        }


    private Mock<UserManager<ApplicationUser>> GetUserManagerMock()
        {
            var storeMock = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                storeMock.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnTrue()
        {
            var userManagerMock = GetUserManagerMock();
            userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Success);

            var service = GetService(userManagerMock.Object);

            var dto = new RegisterDto
            {
                FullName = "Ahmed",
                UserName = "ahmed123",
                Email = "ahmed@example.com",
                Password = "Password123!"
            };

            var result = await service.CreateUserAsync(dto);

            Assert.True(result);
            userManagerMock.Verify(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUserDto_WhenExists()
        {
            var user = new ApplicationUser
            {
                Id = "1",
                FullName = "Ahmed",
                UserName = "ahmed123",
                Email = "ahmed@example.com"
            };

            var userManagerMock = GetUserManagerMock();
            userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);

            var service = GetService(userManagerMock.Object);

            var result = await service.GetUserByIdAsync("1");

            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal("Ahmed", result.FullName);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            var userManagerMock = GetUserManagerMock();
            userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync((ApplicationUser)null!);

            var service = GetService(userManagerMock.Object);

            var result = await service.GetUserByIdAsync("1");

            Assert.Null(result);
        }

        [Fact]
        public async Task AssignRoleAsync_ShouldReturnTrue_WhenUserExists()
        {
            var user = new ApplicationUser { Id = "1" };

            var userManagerMock = GetUserManagerMock();
            userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            userManagerMock.Setup(um => um.AddToRoleAsync(user, "Admin")).ReturnsAsync(IdentityResult.Success);

            var service = GetService(userManagerMock.Object);

            var result = await service.AssignRoleAsync("1", "Admin");

            Assert.True(result);
            userManagerMock.Verify(um => um.AddToRoleAsync(user, "Admin"), Times.Once);
        }

        [Fact]
        public async Task AssignRoleAsync_ShouldReturnFalse_WhenUserNotExists()
        {
            var userManagerMock = GetUserManagerMock();
            userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync((ApplicationUser)null!);

            var service = GetService(userManagerMock.Object);

            var result = await service.AssignRoleAsync("1", "Admin");

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnTrue_WhenUserExists()
        {
            var user = new ApplicationUser
            {
                Id = "1",
                FullName = "Ahmed",
                UserName = "ahmed123",
                Email = "ahmed@example.com"
            };

            var userManagerMock = GetUserManagerMock();
            userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            userManagerMock.Setup(um => um.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var service = GetService(userManagerMock.Object);

            var dto = new UserDto
            {
                Id = "1",
                FullName = "Ali",
                UserName = "ali123",
                Email = "ali@example.com"
            };

            var result = await service.UpdateUserAsync(dto);

            Assert.True(result);
            userManagerMock.Verify(um => um.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnFalse_WhenUserNotExists()
        {
            var userManagerMock = GetUserManagerMock();
            userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync((ApplicationUser)null!);

            var service = GetService(userManagerMock.Object);

            var dto = new UserDto
            {
                Id = "1",
                FullName = "Ali",
                UserName = "ali123",
                Email = "ali@example.com"
            };

            var result = await service.UpdateUserAsync(dto);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserExists()
        {
            var user = new ApplicationUser { Id = "1" };

            var userManagerMock = GetUserManagerMock();
            userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            userManagerMock.Setup(um => um.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            var service = GetService(userManagerMock.Object);

            var result = await service.DeleteUserAsync("1");

            Assert.True(result);
            userManagerMock.Verify(um => um.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnFalse_WhenUserNotExists()
        {
            var userManagerMock = GetUserManagerMock();
            userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync((ApplicationUser)null!);

            var service = GetService(userManagerMock.Object);

            var result = await service.DeleteUserAsync("1");

            Assert.False(result);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnList()
        {
            var users = new List<ApplicationUser>
        {
            new ApplicationUser { Id = "1", FullName = "Ahmed", UserName = "ahmed123", Email = "ahmed@example.com" },
            new ApplicationUser { Id = "2", FullName = "Ali", UserName = "ali123", Email = "ali@example.com" }
        };

            var userManagerMock = GetUserManagerMock();
            userManagerMock.Setup(um => um.Users).Returns(users.AsQueryable());
            userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string> { "Admin" });

            var service = GetService(userManagerMock.Object);

            var result = await service.GetAllUsersAsync();

            Assert.Equal(2, result.Count);
            Assert.All(result, u => Assert.Contains("Admin", u.Role));
        }
    }


}
