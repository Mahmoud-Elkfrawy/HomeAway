using HomeAway.Application.DTOs;
using HomeAway.Application.Interfaces;
using HomeAway.Domain.Entities;
using HomeAway.Domain.Interfaces;
using HomeAway.Domain.Enums;
using HomeAway.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace HomeAway.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;

        public ReservationService(IReservationRepository reservationRepository, IRoomRepository roomRepository)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
        }

        public async Task<bool> CreateReservationAsync(CreateReservationDto dto)
        {
            var room = await _roomRepository.GetByIdAsync(dto.RoomId);
            if (room == null || !room.IsAvailable) return false;

            var reservation = new Reservation
            {
                RoomId = dto.RoomId,
                UserId = dto.UserId,
                DateRange = new DateRange(dto.From, dto.To),
                Status = ReservationStatus.Pending,
                TotalPrice = new Money((dto.To - dto.From).Days * 100, "USD") // example price
            };

            await _reservationRepository.AddAsync(reservation);
            return true;
        }

        public async Task<List<ReservationDto>> GetUserReservationsAsync(string userId)
        {
            var reservations = await _reservationRepository.GetByUserIdAsync(userId);

            var user = await _userManager.FindByIdAsync(userId);

            return reservations.Select(r => new ReservationDto
            {
                Id = r.Id,
                RoomNumber = r.Room.Number,
                UserName = user?.FullName,  // Get from Identity
                From = r.DateRange.From,
                To = r.DateRange.To,
                Status = r.Status.ToString(),
                TotalPrice = r.TotalPrice.Amount
            }).ToList();
        }

    }
}
