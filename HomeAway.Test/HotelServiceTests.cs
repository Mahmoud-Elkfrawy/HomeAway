using HomeAway.Application.DTOs;
using HomeAway.Application.Services;
using HomeAway.Domain.Entities;
using HomeAway.Infrastructure.Data;
using HomeAway.Infrastructure.Persistence; // لازم يكون مشروع HomeAway.Infrastructure مرجّع
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace HomeAway.Test
{
    public class HotelServiceIntegrationTests
    {
        // هذا الدالة تقرأ connection string من appsettings.json في مشروع الاختبارات
        private string GetConnectionString()
        {
            // تأكد أن appsettings.json موجود في HomeAway.Test و Copy if newer
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // قراءة الاتصال بالسيرفر
            var connectionString = config.GetConnectionString("DefaultConnection2");

            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("Connection string not found in appsettings.json");

            return connectionString;
        }

        // هذا الدالة تنشئ DbContext جاهز للاختبار
        private HomeAwayDbContext GetSqlServerDbContext()
        {
            var options = new DbContextOptionsBuilder<HomeAwayDbContext>()
                .UseSqlServer(GetConnectionString())
                .Options;

            return new HomeAwayDbContext(options);
        }

        [Fact]
        public async Task CreateHotel_ShouldAddHotelToDb()
        {
            // إعداد
            using var context = GetSqlServerDbContext();
            var service = new HotelService(new HomeAway.Infrastructure.Repositories.HotelRepository(context));

            var hotelDto = new HotelDto
            {
                Name = "Test Hotel",
                Address = "Test Address",
                Description = "Test Description",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                images = Array.Empty<string>(),
                Rating = 5
            };

            // تنفيذ
            var id = await service.CreateAsync(hotelDto);

            // تحقق
            var addedHotel = await context.Hotels.FindAsync(id);
            Assert.NotNull(addedHotel);
            Assert.Equal("Test Hotel", addedHotel.Name);

            // تنظيف: إزالة الاختبار من قاعدة البيانات
            context.Hotels.Remove(addedHotel);
            await context.SaveChangesAsync();
        }
    }
}
