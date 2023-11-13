using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustDrive.Models
{
    public class Points
    {
        public int PointsId { get; set; }
        public int PointSum { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }

    }
}
