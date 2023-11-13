using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustDrive.Models
{
    public class Reserved
    {
        public int ReservedId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndtDate { get; set; }

        public decimal Price { get; set; }

        public Boolean IsArrive { get; set; }

        public string Status { get; set; }

        public string ReserveType { get; set; }

        //relation

        public Car Car { get; set; }
        public int CarId { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
    }
}
