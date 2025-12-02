using HomeAway.Domain.Interfaces;
using HomeAway.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HomeAwayDbContext _context;

        public IReservationRepository Reservations { get; }
        public IRoomRepository Rooms { get; }
        public IHotelRepository Hotels { get; }

        public UnitOfWork(
            HomeAwayDbContext context,
            IReservationRepository reservationRepository,
            IRoomRepository roomRepository,
            IHotelRepository hotelRepository)
        {
            _context = context;
            Reservations = reservationRepository;
            Rooms = roomRepository;
            Hotels = hotelRepository;
        }
        public Task<int> CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
