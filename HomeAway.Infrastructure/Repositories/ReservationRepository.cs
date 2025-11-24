using HomeAway.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Infrastructure.Repositories
{
    public class ReservationRepository
    {

        private readonly HomeAwayDbContext _context;

        public ReservationRepository(HomeAwayDbContext context)
        {
            _context = context;
        }


        public async Task<bool> IsRoomAvailable(int roomId, DateTime from, DateTime to)
        {
            return !await _context.Reservations
                .AnyAsync(r =>
                    r.RoomId == roomId &&
                    from < r.DateRange.To &&
                    to > r.DateRange.From);
        }

    }
}
