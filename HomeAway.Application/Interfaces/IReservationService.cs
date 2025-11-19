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
        Task<List<ReservationDto>> GetUserReservationsAsync(int userId);
    }
}
