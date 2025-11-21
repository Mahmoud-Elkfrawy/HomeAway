using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HomeAway.Domain.Enums;
using HomeAway.Domain.ValueObjects;

namespace HomeAway.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public String UserId { get; set; }

        public DateRange DateRange { get; set; }
        public Money TotalPrice { get; set; }

        public ReservationStatus Status { get; set; }
    }
}
