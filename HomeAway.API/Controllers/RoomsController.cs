using HomeAway.Application.DTOs;
using HomeAway.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            var rooms = await _roomService.GetAllAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            return room == null ? NotFound() : Ok(room);
        }

        //[Authorize(Roles = "Admin,Provider")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomDto dto)
        {
            var roomId = await _roomService.CreateRoomAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = roomId }, null);
        }

        //[Authorize(Roles = "Admin,Provider")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateRoomDto dto)
        {
            var updated = await _roomService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        //[Authorize(Roles = "Admin,Provider")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _roomService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
