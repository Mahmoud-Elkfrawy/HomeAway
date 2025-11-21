using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeAway.Domain.Interfaces;

namespace HomeAway.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IReservationRepository Reservation { get; }
        IRoomRepository Rooms { get; }

        Task<int> CompleteAsync();
    }
}
