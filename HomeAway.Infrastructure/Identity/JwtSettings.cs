using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAway.Infrastructure.Identity
{
    public class JwtSettings
    {
        public string Key { get; set; }          // Secret key used for signing the token
        public string Issuer { get; set; }       // Who issued the token
        public string Audience { get; set; }     // Who the token is intended for
        public int DurationInMinutes { get; set; } // Token expiration time
    }
}
