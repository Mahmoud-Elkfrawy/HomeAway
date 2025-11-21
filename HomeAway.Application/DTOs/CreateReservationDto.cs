using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Application.DTOs
{
    public class CreateReservationDto
    {
        public int RoomId { get; set; }
        public String UserId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
