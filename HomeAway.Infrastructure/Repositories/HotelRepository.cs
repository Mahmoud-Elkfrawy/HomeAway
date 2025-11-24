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
    public class HotelRepository : IHotelRepository
    {
        private readonly HomeAwayDbContext _context;

        public HotelRepository(HomeAwayDbContext context)
        {
            _context = context;
        }

        public async Task<List<Hotel>> GetAllAsync()
        {
            return await _context.Hotels.ToListAsync();
        }

        public async Task<Hotel> GetByIdAsync(int id)
        {
            return await _context.Hotels
                .Include(h => h.Rooms) // optional
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task AddAsync(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Hotel hotel)
        {
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Hotel hotel)
        {
            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
        }

        public async Task<Hotel> GetByNameAsync(string Name)
        {
            return await _context.Hotels.FirstOrDefaultAsync(r => r.Name == Name);

        }

        //public async Task<bool> ExistsAsync(int id)
        //{
        //    return await _context.Hotels.AnyAsync(h => h.Id == id);
        //}
    }
}
