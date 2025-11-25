using HomeAway.Application.DTOs;
using HomeAway.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeAway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var hotels = await _hotelService.GetAllAsync();
            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hotel = await _hotelService.GetByIdAsync(id);
            return hotel == null ? NotFound() : Ok(hotel);
        }

        //[Authorize(Roles = "Provider")]
        [HttpPost]
        public async Task<IActionResult> Create(HotelDto dto)
        {
            var hotelId = await _hotelService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = hotelId }, null);
        }

        //[Authorize(Roles = "Provider")]
        [HttpPut]
        public async Task<IActionResult> Update(HotelDto dto)
        {
            var updated = await _hotelService.UpdateAsync(dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        //[Authorize(Roles = "Provider")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _hotelService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
