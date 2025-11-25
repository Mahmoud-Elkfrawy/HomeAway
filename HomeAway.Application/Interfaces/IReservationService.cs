using HomeAway.Application.DTOs;
using HomeAway.Domain.Entities;
using HomeAway.Domain.Enums;
using HomeAway.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationDto?> BookRoomAsync(ReservationDto dto);
        Task<bool> UpdateAsync(ReservationDto dto);

        //Task<bool> CreateReservationAsync(ReservationDto dto);

        Task<ReservationDto?> GetByIdAsync(int id);

        Task<List<ReservationDto>> GetByUserIdAsync(string userId);

        Task<List<ReservationDto>> GetUserReservationsAsync(string userId);

        Task<bool> IsRoomAvailableAsync(int roomId, DateTime from, DateTime to);
        //Task AddAsync(Reservation reservation);
        Task<List<Reservation>> GetAllAsync();
        Task DeleteAsync(ReservationDto reservation);
    }
}
