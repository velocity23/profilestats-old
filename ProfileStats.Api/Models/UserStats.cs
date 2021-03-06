using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileStats.Api.Models
{
    public class UserStats
    {
        public int OnlineFlights { get; set; }
        public double Xp { get; set; }
        public int LandingCount { get; set; }
        public string FlightTime { get; set; }
        public int AtcOperations { get; set; }
        public string AtcRank { get; set; }
        public int Grade { get; set; }
    }
}
