using HomeAway.Application.DTOs;
using HomeAway.Application.Services;
using HomeAway.Domain.Entities;
using HomeAway.Infrastructure.Data;
using HomeAway.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HomeAway.Test
{
    public class HotelServiceIntegrationTests
    {
        // ------------------- InMemory DbContext -------------------
        private HomeAwayDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<HomeAwayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // كل test database منفصل
                .Options;

            return new HomeAwayDbContext(options);
        }

        // ------------------- Service Helper -------------------
        private HotelService GetService(HomeAwayDbContext context)
        {
            var repo = new HotelRepository(context);
            return new HotelService(repo);
        }

        // ------------------- CREATE TEST -------------------
        [Fact]
        public async Task CreateHotel_ShouldAddHotelToDb()
        {
            using var context = GetInMemoryDbContext();
            var service = GetService(context);

            var dto = new HotelDto
            {
                Name = "Test Hotel",
                Address = "Test Address",
                Description = "Desc",
                Email = "mail@mail.com",
                PhoneNumber = "123456",
                images = Array.Empty<string>(),
                Rating = 4
            };

            var id = await service.CreateAsync(dto);

            var hotel = await context.Hotels.FindAsync(id);
            Assert.NotNull(hotel);
            Assert.Equal("Test Hotel", hotel!.Name);
        }

        // ------------------- GET BY ID TEST -------------------
        [Fact]
        public async Task GetById_ShouldReturnHotel()
        {
            using var context = GetInMemoryDbContext();
            var service = GetService(context);

            var hotel = new Hotel
            {
                Name = "GetTest",
                Address = "A1",
                Description = "D1",
                Email = "e@test.com",
                PhoneNumber = "111",
                images = Array.Empty<string>(),
                Rating = 5
            };

            context.Hotels.Add(hotel);
            await context.SaveChangesAsync();

            var result = await service.GetByIdAsync(hotel.Id);
            Assert.NotNull(result);
            Assert.Equal("GetTest", result!.Name);
        }

        // ------------------- GET ALL TEST -------------------
        [Fact]
        public async Task GetAll_ShouldReturnAllHotels()
        {
            using var context = GetInMemoryDbContext();
            var service = GetService(context);

            context.Hotels.Add(new Hotel { Name = "H1", Address = "A1", Description = "D1", Email = "e1@test.com", PhoneNumber = "111", images = Array.Empty<string>(), Rating = 3 });
            context.Hotels.Add(new Hotel { Name = "H2", Address = "A2", Description = "D2", Email = "e2@test.com", PhoneNumber = "222", images = Array.Empty<string>(), Rating = 5 });

            await context.SaveChangesAsync();

            var hotels = await service.GetAllAsync();
            Assert.Equal(2, hotels.Count);
        }

        // ------------------- UPDATE TEST -------------------
        [Fact]
        public async Task UpdateHotel_ShouldModifyExistingHotel()
        {
            using var context = GetInMemoryDbContext();
            var service = GetService(context);

            var hotel = new Hotel { Name = "Old", Address = "OldA", Description = "OldD", Email = "old@test.com", PhoneNumber = "999", images = Array.Empty<string>(), Rating = 1 };
            context.Hotels.Add(hotel);
            await context.SaveChangesAsync();

            var dto = new UpdateHotelDto
            {
                Id = hotel.Id,
                Name = "New",
                Address = "NewA",
                Description = "NewD",
                Email = "new@test.com",
                PhoneNumber = "123",
                images = Array.Empty<string>(),
                Rating = 2
            };

            var result = await service.UpdateAsync(dto);
            Assert.True(result);

            var updated = await context.Hotels.FindAsync(hotel.Id);
            Assert.NotNull(updated);
            Assert.Equal("New", updated!.Name);
        }

        // ------------------- DELETE TEST -------------------
        [Fact]
        public async Task DeleteHotel_ShouldRemoveHotel()
        {
            using var context = GetInMemoryDbContext();
            var service = GetService(context);

            var hotel = new Hotel { Name = "DeleteMe", Address = "A", Description = "D", Email = "del@test.com", PhoneNumber = "123", images = Array.Empty<string>(), Rating = 3 };
            context.Hotels.Add(hotel);
            await context.SaveChangesAsync();

            var result = await service.DeleteAsync(hotel.Id);
            Assert.True(result);

            var deleted = await context.Hotels.FindAsync(hotel.Id);
            Assert.Null(deleted);
        }
    }
}
