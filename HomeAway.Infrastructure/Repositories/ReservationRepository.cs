using HomeAway.Domain.Entities;
using HomeAway.Domain.Interfaces;
using HomeAway.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly HomeAwayDbContext _context;

        public ReservationRepository(HomeAwayDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }

        public Task<bool> AnyOverlappingAsync(int roomId, DateTime from, DateTime to)
        {
            return _context.Reservations
                .AnyAsync(r =>
                    r.RoomId == roomId &&
                    from <= r.DateRange.To &&
                    to >= r.DateRange.From);
        }

        public async Task DeleteAsync(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }

        public Task<List<Reservation>> GetAllAsync()
        {
            return _context.Reservations
                        .Include(r => r.Room)
                        .ToListAsync();
        }

        public async Task<Reservation> GetByIdAsync(int id)
        {
            return await _context.Reservations.FindAsync(id);
        }

        public async Task<Reservation> GetByNameAsync(string Name)
        {
            return await _context.Reservations.FindAsync(Name);
        }

        public Task<List<Reservation>> GetByRoomIdAsync(int roomId)
        {
            return _context.Reservations.Where(r => r.RoomId == roomId).ToListAsync();
        }

        public Task<List<Reservation>> GetByUserIdAsync(string userId)
        {
            return _context.Reservations.Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task<bool> IsRoomAvailable(int roomId, DateTime from, DateTime to)
        {
            return !await _context.Reservations
                .AnyAsync(r =>
                    r.RoomId == roomId &&
                    from < r.DateRange.To &&
                    to > r.DateRange.From);
        }

        public async Task UpdateAsync(Reservation reservation)
        {
             _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }
    }
}
