using HomeAway.Application.DTOs;
using HomeAway.Application.Services;
using HomeAway.Domain.Entities;
using HomeAway.Domain.Enums;
using HomeAway.Domain.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HomeAway.Test
{
    public class RoomServiceTests
    {
        // ------------------- Helper to create service -------------------
        private RoomService GetService(Mock<IRoomRepository> roomRepoMock)
        {
            return new RoomService(roomRepoMock.Object);
        }


    // ------------------- CreateRoomAsync Test -------------------
    [Fact]
        public async Task CreateRoomAsync_ShouldReturnTrue()
        {
            var roomRepoMock = new Mock<IRoomRepository>();
            roomRepoMock.Setup(r => r.AddAsync(It.IsAny<Room>())).Returns(Task.CompletedTask);

            var service = GetService(roomRepoMock);

            var dto = new RoomDto
            {
                Quantity = 2,
                Type = RoomType.Double,
                Price = 150,
                HotelId = 1,
                Number = "101",
                IsAvailable = true
            };

            var result = await service.CreateRoomAsync(dto);

            Assert.True(result);
            roomRepoMock.Verify(r => r.AddAsync(It.IsAny<Room>()), Times.Once);
        }

        // ------------------- GetAllAsync Test -------------------
        [Fact]
        public async Task GetAllAsync_ShouldReturnRooms()
        {
            var rooms = new List<Room>
        {
            new Room { Id = 1, Number = "101", Type = RoomType.Single, Quantity = 1, Price = 100, IsAvailable = true },
            new Room { Id = 2, Number = "102", Type = RoomType.Double, Quantity = 2, Price = 200, IsAvailable = true }
        };

            var roomRepoMock = new Mock<IRoomRepository>();
            roomRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(rooms);

            var service = GetService(roomRepoMock);

            var result = await service.GetAllAsync();

            Assert.Equal(2, result.Count);
        }

        // ------------------- GetRoomByIdAsync Tests -------------------
        [Fact]
        public async Task GetRoomByIdAsync_ShouldReturnRoomDto_WhenExists()
        {
            var room = new Room { Id = 1, Number = "101", Type = RoomType.Single, Quantity = 1, Price = 100, IsAvailable = true, HotelId = 1 };

            var roomRepoMock = new Mock<IRoomRepository>();
            roomRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(room);

            var service = GetService(roomRepoMock);

            var result = await service.GetRoomByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("101", result.Number);
        }

        [Fact]
        public async Task GetRoomByIdAsync_ShouldReturnDefault_WhenNotExists()
        {
            var roomRepoMock = new Mock<IRoomRepository>();
            roomRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Room)null!);

            var service = GetService(roomRepoMock);

            var result = await service.GetRoomByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
            Assert.Equal(string.Empty, result.Number);
        }

        // ------------------- UpdateAsync Tests -------------------
        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedDto_WhenRoomExists()
        {
            var room = new Room { Id = 1, Number = "101", Type = RoomType.Single, Quantity = 1, Price = 100, IsAvailable = true, HotelId = 1 };

            var roomRepoMock = new Mock<IRoomRepository>();
            roomRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(room);
            roomRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Room>())).Returns(Task.CompletedTask);

            var service = GetService(roomRepoMock);

            var dto = new RoomDto
            {
                Id = 1,
                Number = "102",
                Type = RoomType.Double,
                Quantity = 2,
                Price = 200,
                IsAvailable = false,
                HotelId = 1
            };

            var result = await service.UpdateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("102", result.Number);
            roomRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Room>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnDefault_WhenRoomNotExists()
        {
            var roomRepoMock = new Mock<IRoomRepository>();
            roomRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Room)null!);

            var service = GetService(roomRepoMock);

            var dto = new RoomDto
            {
                Id = 1,
                Number = "102",
                Type = RoomType.Double,
                Quantity = 2,
                Price = 200,
                IsAvailable = false,
                HotelId = 1
            };

            var result = await service.UpdateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
            Assert.Equal(string.Empty, result.Number);
        }

        // ------------------- DeleteAsync Tests -------------------
        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenRoomExists()
        {
            var room = new Room { Id = 1 };

            var roomRepoMock = new Mock<IRoomRepository>();
            roomRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(room);
            roomRepoMock.Setup(r => r.DeleteAsync(room)).Returns(Task.CompletedTask);

            var service = GetService(roomRepoMock);

            var result = await service.DeleteAsync(1);

            Assert.True(result);
            roomRepoMock.Verify(r => r.DeleteAsync(room), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenRoomNotExists()
        {
            var roomRepoMock = new Mock<IRoomRepository>();
            roomRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Room)null!);

            var service = GetService(roomRepoMock);

            var result = await service.DeleteAsync(1);

            Assert.False(result);
        }
    }


}
