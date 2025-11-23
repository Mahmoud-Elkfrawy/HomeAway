using HomeAway.Application.DTOs;
using HomeAway.Application.Interfaces;
using HomeAway.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeAway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var rooms = await _roomService.GetAllAsync();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var room = await _roomService.GetRoomByIdAsync(id);
                return room == null ? NotFound() : Ok(room);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin,Provider")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomDto dto)
        {
            try
            {
                var roomId = await _roomService.CreateRoomAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = roomId }, null);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin,Provider")]
        [HttpPut]
        public async Task<IActionResult> Update(RoomDto dto)
        {
            try
            {
                var updated = await _roomService.UpdateAsync(dto);

                if (updated == null)
                    return NotFound();

                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }

        }

        //[Authorize(Roles = "Admin,Provider")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool deleted = await _roomService.DeleteAsync(id);

                if (!deleted)
                    return NotFound();

                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }

        }
    }
}
