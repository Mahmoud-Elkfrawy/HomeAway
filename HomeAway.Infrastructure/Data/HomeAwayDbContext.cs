using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeAway.Infrastructure;
using Microsoft.AspNetCore.Identity;
using HomeAway.Domain.Entities;
using HomeAway.Infrastructure.Identity;
using HomeAway.Domain.ValueObjects;

namespace HomeAway.Infrastructure.Data
{
    public class HomeAwayDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public HomeAwayDbContext(DbContextOptions<HomeAwayDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Room-Hotel relationship
            modelBuilder.Entity<Room>()
                .HasOne<Hotel>()                   // Room has 1 Hotel
                .WithMany(h => h.Rooms)            // Hotel has many Rooms
                .HasForeignKey(r => r.HotelId);    // FK on Room

            // Reservation relationships
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany()
                .HasForeignKey(r => r.RoomId);

            // Owned types
            modelBuilder.Entity<Reservation>(r =>
            {
                r.OwnsOne(x => x.DateRange, dr =>
                {
                    dr.Property(x => x.From).HasColumnName("From");
                    dr.Property(x => x.To).HasColumnName("To");
                });


            });
            modelBuilder.Entity<Reservation>()
                .Property(r => r.TotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Room>()
                .Property(r => r.Price)
                .HasPrecision(18, 2);
        }

    }
}
