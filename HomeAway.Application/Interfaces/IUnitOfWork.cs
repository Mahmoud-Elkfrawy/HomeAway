using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Application.Interfaces
{
    public interface IUnitOfWork
    {
        //IBookingRepository Bookings { get; }
        //IRoomRepository Rooms { get; }
        //IUserRepository Users { get; }

        Task<int> CompleteAsync();
    }
}
