using HomeAway.Application.DTOs;
using HomeAway.Application.Interfaces;
using HomeAway.Domain.Entities;
using HomeAway.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeAway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var reservations = await _reservationService.GetAllAsync();
                return Ok(reservations);
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
                var reservation = await _reservationService.GetByIdAsync(id);
                if (reservation == null) return NotFound();
                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(ReservationDto dto)
        {
            try
            {
                var result = await _reservationService.BookRoomAsync(dto);
                if (result == false)
                {
                    return BadRequest();
                }
                return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ReservationDto dto)
        {
            try
            {
                var existing = await _reservationService.GetByIdAsync(id);

                if (existing == null) return NotFound();

                existing.RoomId = dto.RoomId;
                existing.From = dto.From;
                existing.To = dto.To;
                existing.Status = dto.Status;
                existing.TotalPrice = dto.TotalPrice;
                await _reservationService.UpdateAsync(existing);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var reservation = await _reservationService.GetByIdAsync(id);
                if (reservation == null) return NotFound();

                await _reservationService.DeleteAsync(reservation);
                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}
