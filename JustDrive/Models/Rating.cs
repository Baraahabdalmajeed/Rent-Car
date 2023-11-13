using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustDrive.Models
{
    public class Rating
    {
        public int RatingId { get; set; }

        public int StarsPoints { get; set; }


        //relation
        public User User { get; set; }
        public string UserId { get; set; }
    }
}
