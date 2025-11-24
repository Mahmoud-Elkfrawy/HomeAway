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
            var reservations = await _reservationService.GetAllAsync();
            return Ok(reservations);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null) return NotFound();
            return Ok(reservation);
        }


        [HttpPost]
        public async Task<IActionResult> Create(ReservationDto dto)
        {
            var reservation = new Reservation
            {
                RoomId = dto.RoomId,
                UserId = dto.UserId,
                DateRange = new DateRange(dto.From, dto.To),
                Status = dto.Status,
                TotalPrice = new Money(dto.TotalPrice, "USD")
            };
            await _reservationService.AddAsync(reservation);
            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateReservationDto dto)
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null) return NotFound();


            await _reservationService.DeleteAsync(reservation);
            return NoContent();
        }
    }
}
