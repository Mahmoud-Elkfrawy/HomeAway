using HomeAway.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Application.DTOs
{
    public class CreateReservationDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string? UserId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
