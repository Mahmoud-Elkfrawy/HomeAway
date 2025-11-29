using HomeAway.Domain.Entities;
using HomeAway.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using HomeAway.Domain.Interfaces;
using HomeAway.Infrastructure.Data;

namespace HomeAway.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeAwayDbContext _context;

        public RoomRepository(HomeAwayDbContext context)
        {
            _context = context;
        }
        
        public async Task AddAsync(Room entity)
        {
            await _context.Rooms.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Room entity)
        {
            _context.Rooms.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Room>> GetAllAsync()
        {
            return await _context.Rooms        // if you have Hotel navigation
                .ToListAsync();
        }

        public async Task<Room> GetByIdAsync(int id)
        {
            return await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Room> GetByNameAsync(string name)
        {
            return await _context.Rooms
                .FirstOrDefaultAsync(r => r.Number == name);
        }

        public async Task UpdateAsync(Room entity)
        {
            _context.Rooms.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
