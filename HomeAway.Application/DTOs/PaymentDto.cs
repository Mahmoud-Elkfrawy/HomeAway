using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Application.DTOs
{
    public class PaymentDto
    {
        public string UserId { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
        public int CardNumber { get; set; }
        public string Expiry { get; set; } = string.Empty;
        public int CVV { get; set; }
    }
}
