
using HomeAway.Application.Interfaces;
using HomeAway.Application.Services;
using HomeAway.Domain.Interfaces;
using HomeAway.Infrastructure.Data;
using HomeAway.Infrastructure.Identity;
using HomeAway.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace HomeAway.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddDbContext<HomeAwayDbContext>(Options =>
Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection2"),
        b => b.MigrationsAssembly("HomeAway.Infrastructure")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<HomeAwayDbContext>().AddDefaultTokenProviders();

            Console.WriteLine("DB => " + builder.Configuration.GetConnectionString("DefaultConnection2"));



            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
                    var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

                    options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true
                        };
                    });
            builder.Services.AddScoped<HomeAway.Infrastructure.Identity.JwtTokenService>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            //builder.Services.AddScoped<IHotelService, HotelService>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            //builder.Services.AddScoped<IHotelRepository, HotelRepository>();




            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
