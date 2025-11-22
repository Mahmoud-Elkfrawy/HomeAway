using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Domain.ValueObjects
{
    public class DateRange
    {
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }

        private DateRange() { }  // EF Core needs this to create the object


        public DateRange(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }

        //public int TotalNights => (To - From).Days;
    }
}
