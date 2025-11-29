using HomeAway.Domain.Enums;
using HomeAway.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public int? HotelId { get; set; }
        public string? Number { get; set; }
        public RoomType Type { get; set; }
        public int Quantity { get; set; }
        public Decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
