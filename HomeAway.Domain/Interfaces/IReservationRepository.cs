using HomeAway.Application.Interfaces;
using HomeAway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Domain.Interfaces
{
    public interface IReservationRepository: IGenericRepository<Reservation>
    {
        public Task<List<Reservation>> GetByUserIdAsync(String userId);
        Task<List<Reservation>> GetByRoomIdAsync(int roomId);
        Task<bool> AnyOverlappingAsync(int roomId, DateTime from, DateTime to);
        Task<List<Reservation>> GetAllAsync();
        Task<Reservation> GetByIdAsync(int id);
        Task AddAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
        Task DeleteAsync(Reservation reservation);
    }
}
