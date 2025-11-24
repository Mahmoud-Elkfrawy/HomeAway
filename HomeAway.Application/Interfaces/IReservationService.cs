using HomeAway.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Application.Interfaces
{
    public interface IReservationService
    {
        Task<bool> CreateReservationAsync(CreateReservationDto dto);
        Task<List<ReservationDto>> GetUserReservationsAsync(String userId);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime from, DateTime to);
        Task<ReservationDto?> BookRoomAsync(CreateReservationDto dto);
        Task<ReservationDto?> GetByIdAsync(int id);
        Task<List<ReservationDto>> GetByUserIdAsync(string userId);
    }
}
