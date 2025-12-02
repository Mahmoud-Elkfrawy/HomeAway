using HomeAway.Application.DTOs;
using HomeAway.Application.Interfaces;
using HomeAway.Domain.Entities;
using HomeAway.Domain.Enums;
using HomeAway.Domain.Interfaces;
using HomeAway.Domain.ValueObjects;
using HomeAway.Infrastructure.Identity;
using HomeAway.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;


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

        public async Task<bool> BookRoomAsync(CreateReservationDto dto)
        {
            if (!await IsRoomAvailableAsync(dto.RoomId, dto.From, dto.To))
                return false; // or throw a domain exception

            Room room = await _roomRepository.GetByIdAsync(dto.RoomId);
            decimal pricePerNight = room.Price;
            var nights = (int)(dto.To.Date - dto.From.Date).TotalDays;
            var total = pricePerNight * Math.Max(0, nights);


            var reservation = new Reservation
            {
                RoomId = dto.RoomId,
                UserId = dto.UserId,
                DateRange = new DateRange(dto.From, dto.To),
                Status = ReservationStatus.Pending,
                TotalPrice = total,
            };


            await _reservationRepository.AddAsync(reservation);

            return true;
        }
        #region MyRegion

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
        #endregion

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
                TotalPrice = r.TotalPrice
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
                TotalPrice = r.TotalPrice
            }).ToList();
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime from, DateTime to)
        {
            if (from >= to) return false;
            return !await _reservationRepository.AnyOverlappingAsync(roomId, from, to);
        }

        public async Task<bool> UpdateAsync(UpdateResrvationDto dto)
        {
            await _reservationRepository.UpdateAsync(new Reservation
            {
                Id = dto.Id,
                //RoomId = dto.RoomId,
                //UserId = dto.UserId,
                DateRange = new DateRange(dto.From, dto.To),
                Status = dto.Status,
                //TotalPrice = dto.TotalPrice
            });
            return true;
        }
        public async Task<List<Reservation>> GetAllAsync()
        {
            return await _reservationRepository.GetAllAsync();

        }
        public async Task DeleteAsync(ReservationDto reservation)
        {
            Reservation reservation1 = await _reservationRepository.GetByIdAsync(reservation.Id);
            await _reservationRepository.DeleteAsync(reservation1);
        }
        public async Task<decimal> HomeAwayProfit()
        {
            List<Reservation> reservation = await _reservationRepository.GetAllAsync();
            //decimal total = 0;
            decimal total = reservation
                    .Where(r => r.Status == ReservationStatus.Confirmed || r.Status == ReservationStatus.Completed)
                    .Sum(r => r.TotalPrice);
            return total * 0.1m;
        }

    }
}
