using HomeAway.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Application.DTOs
{
    public class UpdateRoomDto
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public RoomType Type { get; set; }
        public int Quantity { get; set; }
        public Decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
