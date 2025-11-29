using HomeAway.Application.DTOs;
using HomeAway.Application.Interfaces;
using HomeAway.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
            try
            {
                var hotels = await _hotelService.GetAllAsync();
                return Ok(hotels);
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
                var hotel = await _hotelService.GetByIdAsync(id);
                return hotel == null ? NotFound() : Ok(hotel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        //[Authorize(Roles = "Provider")]
        [HttpPost]
        public async Task<IActionResult> Create(HotelDto dto)
        {
            try
            {
                var hotelId = await _hotelService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = hotelId }, null);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        //[Authorize(Roles = "Provider")]
        [HttpPut]
        public async Task<IActionResult> Update(HotelDto dto)
        {
            try
            {
                var updated = await _hotelService.UpdateAsync(dto);
                if (!updated)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        //[Authorize(Roles = "Provider")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _hotelService.DeleteAsync(id);
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
