using HomeAway.Application.Auth;
using HomeAway.Application.DTOs;
using HomeAway.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeAway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtTokenService _jwtTokenService;


        public ProvidersController(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        JwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenService = jwtTokenService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    FullName = dto.FullName
                };

                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded) return BadRequest(result.Errors);


                if (!await _roleManager.RoleExistsAsync("Provider"))
                    await _roleManager.CreateAsync(new IdentityRole("Provider"));


                await _userManager.AddToRoleAsync(user, "Provider");


                return Ok("Provider created successfully.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(dto.UserName);
                if (user == null) return Unauthorized();


                if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                    return Unauthorized();


                //var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtTokenService.GenerateToken(user);


                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }

        }
    }
}
