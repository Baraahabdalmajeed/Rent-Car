using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustDrive.Models
{
    public class StationViewModel
    {
        public string StationName { get; set; }
        public string EmpName { get; set; }
        public string PhoneNumber { get; set; }
        public int CarsCount { get; set; }

        public Car Cars { get; set; }

    }
}
