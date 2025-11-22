using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Domain.ValueObjects
{
    public class Money
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        private Money() { }   // EF Core requires this
        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }
    }
}
