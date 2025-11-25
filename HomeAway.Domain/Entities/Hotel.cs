using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Domain.Entities
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; }  = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string[]? images { get; set; } = Array.Empty<string>();
        public int Rating { get; set; }
        public List<Room> Rooms { get; set; } = new();
    }
}
