using JustDrive.Data;
using JustDrive.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustDrive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingApiController : ControllerBase
    {
        private ApplicationDbContext _context;
        public RatingApiController(ApplicationDbContext db)
        {
            _context = db;
        }


        public class RateViewModel
        {
            public string UserId { get; set; }
            public int StarsPoints { get; set; }

        }

        [HttpPost]
        [Route("Rate")]
        public string Rate(RateViewModel rateViewModel)
        {
            var User = _context.Users.FirstOrDefault(u => u.Id == rateViewModel.UserId);

            if (User == null)
            {
                return "User Not Found";
            }

            Rating rate = new Rating
            {
                UserId = User.Id,
                StarsPoints = rateViewModel.StarsPoints
            };
            _context.Rating.Add(rate);
            _context.SaveChanges();
            return "Done";
        }


    }
}
