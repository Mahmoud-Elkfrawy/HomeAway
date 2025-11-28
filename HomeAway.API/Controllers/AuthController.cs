using HomeAway.Application.Auth;
using HomeAway.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeAway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtTokenService _jwtService;

        public AuthController(UserManager<ApplicationUser> userManager, JwtTokenService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    FullName = dto.FullName
                };

                var result = await _userManager.CreateAsync(user, dto.Password);

                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                return Ok("User Registered Successfully");
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

                if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                    return Unauthorized("Invalid username or password");

                var token = _jwtService.GenerateToken(user);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}
