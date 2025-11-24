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
using HomeAway.Infrastructure.Identity;
using HomeAway.Infrastructure.Repositories;


namespace HomeAway.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly UserManager<ApplicationUser> _userManager;


        public ReservationService(IReservationRepository reservationRepository, IRoomRepository roomRepository, UserManager<ApplicationUser> userManager)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
            _userManager = userManager;
        }

        public async Task<ReservationDto?> BookRoomAsync(ReservationDto dto)
        {
            if (!await IsRoomAvailableAsync(dto.RoomId, dto.From, dto.To))
                return null; // or throw a domain exception


            decimal pricePerNight = await _roomRepository.GetByIdAsync(dto.RoomId).Price.Amount; 
            var nights = (int)(dto.To.Date - dto.From.Date).TotalDays;
            var total = pricePerNight * Math.Max(0, nights);


            var reservation = new Reservation
            {
                RoomId = dto.RoomId,
                UserId = dto.UserId,
                DateRange = new DateRange(dto.From,dto.To),
                Status = ReservationStatus.Pending,
                TotalPrice = new Money(total, "USD"),
            };


            await _reservationRepository.AddAsync(reservation);


            return new ReservationDto
            {
                Id = reservation.Id,
                RoomId = reservation.RoomId,
                UserId = reservation.UserId,
                From = reservation.DateRange.From,
                To = reservation.DateRange.To,
                TotalPrice = reservation.TotalPrice.Amount
            };
        }

        //public async Task<bool> CreateReservationAsync(ReservationDto dto)
        //{
        //    var room = await _roomRepository.GetByIdAsync(dto.RoomId);
        //    if (room == null || !room.IsAvailable) return false;

        //    var reservation = new Reservation
        //    {
        //        RoomId = dto.RoomId,
        //        UserId = dto.UserId,
        //        DateRange = new DateRange(dto.From, dto.To),
        //        Status = ReservationStatus.Pending,
        //        TotalPrice = new Money((dto.To - dto.From).Days * room.Price.Amount, "USD")
        //    };

        //    await _reservationRepository.AddAsync(reservation);
        //    return true;
        //}

        public async Task<ReservationDto?> GetByIdAsync(int id)
        {
            var r = await _reservationRepository.GetByIdAsync(id);
            if (r == null) return null;
            return new ReservationDto
            {
                Id = r.Id,
                RoomId = r.RoomId,
                UserId = r.UserId,
                From = r.DateRange.From,
                To = r.DateRange.To,
                TotalPrice = r.TotalPrice.Amount
            };
        }

        public Task<List<ReservationDto>> GetByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ReservationDto>> GetUserReservationsAsync(string userId)
        {
            var reservations = await _reservationRepository.GetByUserIdAsync(userId);

            var user = await _userManager.FindByIdAsync(userId);

            return reservations.Select(r => new ReservationDto
            {
                Id = r.Id,
                //RoomNumber = r.Room.Number,
                //UserName = user?.FullName,
                From = r.DateRange.From,
                To = r.DateRange.To,
                Status = r.Status,
                TotalPrice = r.TotalPrice.Amount
            }).ToList();
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime from, DateTime to)
        {
            if (from >= to) return false;
            return !await _reservationRepository.AnyOverlappingAsync(roomId, from, to);
        }
    }
}
