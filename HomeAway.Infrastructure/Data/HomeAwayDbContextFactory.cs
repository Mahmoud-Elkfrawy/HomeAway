using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System.IO;

namespace HomeAway.Infrastructure.Data
{
    public class HomeAwayDbContextFactory : IDesignTimeDbContextFactory<HomeAwayDbContext>
    {
        public HomeAwayDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../HomeAway.API")) // adjust path
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<HomeAwayDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection2"));

            return new HomeAwayDbContext(optionsBuilder.Options);
        }
    }
}
