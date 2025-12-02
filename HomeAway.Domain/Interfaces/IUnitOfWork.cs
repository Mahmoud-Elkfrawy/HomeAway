using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IReservationRepository Reservations { get; }
        IRoomRepository Rooms { get; }
        IHotelRepository Hotels { get; }
        Task<int> CompleteAsync();  
    }
}
