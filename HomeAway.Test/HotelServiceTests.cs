using HomeAway.Application.DTOs;
using HomeAway.Application.Services;
using HomeAway.Domain.Entities;
using HomeAway.Infrastructure.Data;
using HomeAway.Infrastructure.Repositories; // عدّل حسب Repository عندك
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HomeAway.Test
{
    public class HotelServiceIntegrationTests
    {
        private HomeAwayDbContext GetSqlServerDbContext()
        {
            var options = new DbContextOptionsBuilder<HomeAwayDbContext>()
                .UseSqlServer("Server=AHMEDMAHMOUDRAG\\SQLEXPRESS;Database=HomeAwayDb;Trusted_Connection=True;")
                .Options;

            return new HomeAwayDbContext(options);
        }

        private HotelService GetHotelService(HomeAwayDbContext context)
        {
            var repo = new HotelRepository(context);
            return new HotelService(repo);
        }


        [Fact]
        public async Task CreateHotel_ShouldAddHotelToDb()
        {
            using var context = GetSqlServerDbContext();
            var service = GetHotelService(context);

            var dto = new HotelDto
            {
                Name = "Integration Test Hotel",
                Address = "Cairo",
                Description = "Test Desc",
                Email = "test@hotel.com",
                PhoneNumber = "0100000000",
                images = new string[] { "img1.jpg" },
                Rating = 5
            };

            // Act
            var id = await service.CreateAsync(dto);

            // Assert
            var inserted = await context.Hotels.FindAsync(id);
            Assert.NotNull(inserted);
            Assert.Equal("Integration Test Hotel", inserted.Name);

            // تنظيف البيانات بعد الاختبار
            context.Hotels.Remove(inserted);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateHotel_ShouldModifyHotelInDb()
        {
            using var context = GetSqlServerDbContext();
            var service = GetHotelService(context);

            // جهز بيانات أولية
            var hotel = new Hotel
            {
                Name = "Old Name",
                Address = "Cairo",
                Description = "Desc",
                Email = "old@hotel.com",
                PhoneNumber = "0111111111",
                images = new string[] { "img.jpg" },
                Rating = 3
            };
            context.Hotels.Add(hotel);
            await context.SaveChangesAsync();

            // Act
            var updateDto = new UpdateHotelDto
            {
                Id = hotel.Id,
                Name = "New Name",
                Address = hotel.Address,
                Description = hotel.Description,
                Email = hotel.Email,
                PhoneNumber = hotel.PhoneNumber,
                images = hotel.images
            };
            var result = await service.UpdateAsync(updateDto);

            var updated = await context.Hotels.FindAsync(hotel.Id);

            // Assert
            Assert.True(result);
            Assert.Equal("New Name", updated.Name);

            // تنظيف
            context.Hotels.Remove(updated);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task DeleteHotel_ShouldRemoveHotelFromDb()
        {
            using var context = GetSqlServerDbContext();
            var service = GetHotelService(context);

            // جهز بيانات أولية
            var hotel = new Hotel
            {
                Name = "To Delete",
                Address = "Cairo",
                Description = "Desc",
                Email = "delete@hotel.com",
                PhoneNumber = "0123456789",
                images = new string[] { "img.jpg" },
                Rating = 4
            };
            context.Hotels.Add(hotel);
            await context.SaveChangesAsync();

            // Act
            var result = await service.DeleteAsync(hotel.Id);
            var deleted = await context.Hotels.FindAsync(hotel.Id);

            // Assert
            Assert.True(result);
            Assert.Null(deleted);
        }
    }
}

