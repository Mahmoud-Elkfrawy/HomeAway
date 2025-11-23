using HomeAway.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Application.DTOs
{
    public class CreateRoomDto
    {
        public string Number { get; set; }
        public decimal Price { get; set; }
        public RoomType Type { get; set; }

        public int Quantity { get; set; }

        public String HotelName { get; set; }
    }

}
