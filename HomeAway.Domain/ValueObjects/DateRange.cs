using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Domain.ValueObjects
{
    public class DateRange
    {
        public DateTime From { get; }
        public DateTime To { get; }

        public DateRange(DateTime from, DateTime to)
        {
            if (to <= from)
                throw new ArgumentException("End date must be after start date");

            From = from;
            To = to;
        }

        public int TotalNights => (To - From).Days;
    }
}
