using HomeAway.Application.DTOs;
using HomeAway.Application.Services;
using HomeAway.Domain.Entities;
using HomeAway.Domain.Enums;
using HomeAway.Domain.Interfaces;
using HomeAway.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HomeAway.Test
{
    public class ReservationServiceTests
    {
        // ------------------- Helper to create service -------------------
        private ReservationService GetService(
            Mock<IReservationRepository> reservationRepoMock,
            Mock<IRoomRepository> roomRepoMock,
            Mock<UserManager<ApplicationUser>> userManagerMock)
        {
            return new ReservationService(
                reservationRepoMock.Object,
                roomRepoMock.Object,
                userManagerMock.Object
            );
        }

        private Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            return new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );
        }

        // ------------------- BookRoomAsync Tests -------------------
        [Fact]
        public async Task BookRoomAsync_ShouldReturnTrue_WhenRoomAvailable()
        {
            var room = new Room { Id = 1, Price = 100m };
            var roomRepoMock = new Mock<IRoomRepository>();
            roomRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(room);

            var reservationRepoMock = new Mock<IReservationRepository>();
            reservationRepoMock.Setup(r => r.AnyOverlappingAsync(1, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                               .ReturnsAsync(false);
            reservationRepoMock.Setup(r => r.AddAsync(It.IsAny<Reservation>())).Returns(Task.CompletedTask);

            var userManagerMock = GetMockUserManager();

            var service = GetService(reservationRepoMock, roomRepoMock, userManagerMock);

            var dto = new CreateReservationDto
            {
                RoomId = 1,
                UserId = "user1",
                From = DateTime.Today,
                To = DateTime.Today.AddDays(2)
            };

            var result = await service.BookRoomAsync(dto);

            Assert.True(result);
            reservationRepoMock.Verify(r => r.AddAsync(It.IsAny<Reservation>()), Times.Once);
        }

        [Fact]
        public async Task BookRoomAsync_ShouldReturnFalse_WhenRoomNotAvailable()
        {
            var roomRepoMock = new Mock<IRoomRepository>();
            roomRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Room());

            var reservationRepoMock = new Mock<IReservationRepository>();
            reservationRepoMock.Setup(r => r.AnyOverlappingAsync(1, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                               .ReturnsAsync(true);

            var userManagerMock = GetMockUserManager();

            var service = GetService(reservationRepoMock, roomRepoMock, userManagerMock);

            var dto = new CreateReservationDto
            {
                RoomId = 1,
                UserId = "user1",
                From = DateTime.Today,
                To = DateTime.Today.AddDays(2)
            };

            var result = await service.BookRoomAsync(dto);

            Assert.False(result);
            reservationRepoMock.Verify(r => r.AddAsync(It.IsAny<Reservation>()), Times.Never);
        }

        // ------------------- GetByIdAsync Test -------------------
        [Fact]
        public async Task GetByIdAsync_ShouldReturnReservation_WhenExists()
        {
            var reservation = new Reservation
            {
                Id = 1,
                RoomId = 5,
                UserId = "user1",
                DateRange = new Domain.ValueObjects.DateRange(DateTime.Today, DateTime.Today.AddDays(1)),
                Status = ReservationStatus.Pending,
                TotalPrice = 100m
            };

            var reservationRepoMock = new Mock<IReservationRepository>();
            reservationRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(reservation);

            var service = GetService(reservationRepoMock, new Mock<IRoomRepository>(), GetMockUserManager());

            var result = await service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result!.Id);
            Assert.Equal(100m, result.TotalPrice);
        }

        // ------------------- GetAllAsync Test -------------------
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllReservations()
        {
            var reservations = new List<Reservation>
            {
                new Reservation { Id = 1, RoomId = 1, UserId = "u1", DateRange = new Domain.ValueObjects.DateRange(DateTime.Today, DateTime.Today.AddDays(1)), Status = ReservationStatus.Pending, TotalPrice = 50 },
                new Reservation { Id = 2, RoomId = 2, UserId = "u2", DateRange = new Domain.ValueObjects.DateRange(DateTime.Today, DateTime.Today.AddDays(2)), Status = ReservationStatus.Confirmed, TotalPrice = 100 }
            };

            var reservationRepoMock = new Mock<IReservationRepository>();
            reservationRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(reservations);

            var service = GetService(reservationRepoMock, new Mock<IRoomRepository>(), GetMockUserManager());

            var result = await service.GetAllAsync();

            Assert.Equal(2, result.Count);
        }

        // ------------------- UpdateAsync Test -------------------
        [Fact]
        public async Task UpdateAsync_ShouldCallRepositoryUpdate()
        {
            var reservationRepoMock = new Mock<IReservationRepository>();
            reservationRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Reservation>())).Returns(Task.CompletedTask);

            var service = GetService(reservationRepoMock, new Mock<IRoomRepository>(), GetMockUserManager());

            var dto = new UpdateResrvationDto
            {
                Id = 1,
                From = DateTime.Today,
                To = DateTime.Today.AddDays(2),
                Status = ReservationStatus.Confirmed
            };

            var result = await service.UpdateAsync(dto);

            Assert.True(result);
            reservationRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Reservation>()), Times.Once);
        }

        // ------------------- DeleteAsync Test -------------------
        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDelete()
        {
            var reservation = new Reservation { Id = 1 };
            var reservationRepoMock = new Mock<IReservationRepository>();
            reservationRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(reservation);
            reservationRepoMock.Setup(r => r.DeleteAsync(reservation)).Returns(Task.CompletedTask);

            var service = GetService(reservationRepoMock, new Mock<IRoomRepository>(), GetMockUserManager());

            var dto = new ReservationDto { Id = 1 };

            await service.DeleteAsync(dto);

            reservationRepoMock.Verify(r => r.DeleteAsync(reservation), Times.Once);
        }

        // ------------------- HomeAwayProfit Test -------------------
        [Fact]
        public async Task HomeAwayProfit_ShouldReturnCorrectProfit()
        {
            var reservations = new List<Reservation>
            {
                new Reservation { Id = 1, TotalPrice = 100, Status = ReservationStatus.Confirmed },
                new Reservation { Id = 2, TotalPrice = 50, Status = ReservationStatus.Pending },
                new Reservation { Id = 3, TotalPrice = 200, Status = ReservationStatus.Completed }
            };

            var reservationRepoMock = new Mock<IReservationRepository>();
            reservationRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(reservations);

            var service = GetService(reservationRepoMock, new Mock<IRoomRepository>(), GetMockUserManager());

            var profit = await service.HomeAwayProfit();

            // 10% of (100 + 200) = 30
            Assert.Equal(30m, profit);
        }
    }
}
