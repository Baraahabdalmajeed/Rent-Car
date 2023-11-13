using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustDrive.Models
{
    public class Station
    {
        public int StationId { get; set; }

        public int StationNumber { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public Boolean IsActive { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }

        public Car Car { get; set; }
        public int CarId { get; set; }
    }
}
