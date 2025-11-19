using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Application.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string HotelName { get; set; }
        public int Capacity { get; set; }
        public string Type { get; set; }
        public bool IsAvailable { get; set; }
    }
}
