using HomeAway.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeAway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        public IUserService _userService { get; set; }
        public AdminController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {

            var users = _userService.GetAllUsersAsync();

            return Ok(users);
        }
    }
}
