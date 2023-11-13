using JustDrive.Data;
using JustDrive.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static JustDrive.Controllers.StationsApiController;

namespace JustDrive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReserveApiController : ControllerBase
    {
        private ApplicationDbContext _context;
        private UserManager<User> userManager;

        public class ReserveViewModel
        {
            public int CarId { get; set; }
            public string UserId { get; set; }
        }


        public class ConfirmReserveViewModel
        {
            public int CarId { get; set; }
            public string UserId { get; set; }
            public int ReserveId { get; set; }

        }

        public class EndViewModel
        {
            public string UserId { get; set; }
            public int ReservedId { get; set; }
            public string ResType { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string EmployeeNumber { get; set; }
        }

        class ShowEmployeeReserveViewModel
        {
            public int CarId { get; set; }
            public string CarName { get; set; }
            public string ReserveType { get; set; }
            public decimal Price { get; set; }
            public string UserId { get; set; }
            public int ReserveId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }

        public class UploadImage
        {
            public int ReserveId { get; set; }
            public string Front { get; set; }
            public string Back { get; set; }
            public string Right { get; set; }
            public string Left { get; set; }

        }

        public ReserveApiController(ApplicationDbContext db , UserManager<User> user)
        {
            _context = db;
            userManager = user;
        }

        [HttpPost]
        [Route("reserve")]

        public object Reserve(ReserveViewModel reserveViewModel)
        {
            var Car = _context.Car.FirstOrDefault(c => c.CarId == reserveViewModel.CarId);
            if (Car == null)
            {
                return "Car Not Found";
            }
            var User = _context.Users.ToList().FirstOrDefault(u => u.Id.Equals(reserveViewModel.UserId));
            if (User == null)
            {
                return "User Not Found";
            }

            if (User != null && Car != null)
            {
                Reserved reserved = new Reserved
                {
                    UserId = User.Id,
                    CarId = Car.CarId,
                    Status = "part",
                    Price = 0,
                    IsArrive = false
                };
                _context.Reserved.Add(reserved);
                _context.SaveChanges();
                return reserved.ReservedId;
            }
            return "reserve failed";

        }

        [HttpPost]
        [Route("ConfirmReserve")]

        public string ConfirmReserve(ConfirmReserveViewModel confirmReserveViewModel)
        {
            var Car = _context.Car.FirstOrDefault(c => c.CarId == confirmReserveViewModel.CarId);
            if (Car == null)
            {
                return "Car Not Found";
            }

            var User = _context.Users.FirstOrDefault(u => u.Id == confirmReserveViewModel.UserId);
            if (User == null)
            {
                return "User Not Found";
            }

            var Reserve = _context.Reserved.FirstOrDefault(r => r.ReservedId == confirmReserveViewModel.ReserveId);
            if (Reserve == null)
            {
                return "Reserve Not Found";
            }

            Reserve.Status = "reserved";

            _context.SaveChanges();

            return "Reserved";
        }

        [HttpPost]
        [Route("EndSession")]
        public object EndSession(EndViewModel endViewModel)
        {
            var reserved = _context.Reserved.Include(c=>c.Car).FirstOrDefault(r => r.ReservedId == endViewModel.ReservedId);
            if(reserved == null)
            {
                return BadRequest("Reserve Not found");
            }

            reserved.StartDate = endViewModel.StartTime;
            reserved.EndtDate = endViewModel.EndTime;
            
            _context.SaveChanges();

         
            TimeSpan End = reserved.EndtDate - reserved.StartDate;

            if (endViewModel.ResType.Equals("minutes".ToLower()))
            {
                reserved.Price = Math.Round(Convert.ToDecimal(End.TotalMinutes) * reserved.Car.PricePerMinutes);

            }
            else if (endViewModel.ResType.Equals("hour".ToLower()))
            {
               reserved.Price = Math.Round(Convert.ToDecimal(End.TotalHours) * reserved.Car.PricePerHour);
            }

            else if (endViewModel.ResType.Equals("day".ToLower()))
            {
                reserved.Price = Math.Round(Convert.ToDecimal(End.TotalDays) * reserved.Car.PricePerDay);
            }
            else
            {
                return BadRequest("Bad type should be (minutes/hour/day)");
            }

            _context.SaveChanges();

            reserved.Status = "free";
            _context.SaveChanges();

            //---------------------------------------------------------------

            var User = _context.Users.Where(u => u.Id == endViewModel.UserId).FirstOrDefault();
            if(User == null)
            {
                return BadRequest("User Not Found");
            }

            var Calculate = _context.Reserved.Where(r => r.UserId == User.Id).ToList();
            if (Calculate == null)
            {
                return BadRequest("Reserve Not Found");
            }

            Points points = new Points
            {
                UserId = User.Id,
                PointSum = Calculate.Count
            };
            _context.Points.Add(points);
            _context.SaveChanges();

            if (Calculate.Count == 11)
            {
       

                reserved.Price = (reserved.Price * 10) / 100;

                points.PointSum = 0;
                _context.SaveChanges();
                
            }

            //----------------------------------------------------------

            var ReturnStation = reserved.Car;
            
            var Station = _context.Station.Where(a => a.CarId == ReturnStation.CarId).FirstOrDefault();
            if(Station == null)
            {
                return BadRequest("Station Not Found");
            }

            var EmpST = _context.Station.FirstOrDefault(s => s.UserId.Equals(endViewModel.EmployeeNumber));

            if (EmpST == null)
            {
                return BadRequest("Employee Not in this Station ");
            }

            EmpST.CarId = ReturnStation.CarId;

            _context.SaveChanges();


            return reserved.Price;
        }


        [HttpPost]
        [Route("UploadImagesCar")]
        public object UploadImagesCar(UploadImage uploadImage)
        {
            var res = _context.Reserved.Include(c=>c.Car).FirstOrDefault(r => r.ReservedId == uploadImage.ReserveId);
            if (res != null)
            {
                var car = res.Car;
                if (car != null)
                {


                    Image i1 = new Image
                    {
                        Path = uploadImage.Front,
                        CarId = car.CarId
                    };
                    Image i2 = new Image
                    {
                        Path = uploadImage.Back,
                         CarId = car.CarId

                    };
                    Image i3 = new Image
                    {
                        Path = uploadImage.Left,
                         CarId = car.CarId
                    };
                    Image i4 = new Image
                    {
                        Path = uploadImage.Right,
                         CarId = car.CarId
                    };

                     _context.SaveChanges();

                        return "Done";
                    }

                else
                {
                    return "Car Not Found";
                }
            }
            else
            {
                return "Reserve Not Found";
            }

        }


        [HttpGet]
        [Route("EmployeeReserves")]
        public async Task <IActionResult> EmployeeReserves(string Id)
        {

            var exsit = await userManager.FindByIdAsync(Id);

            if (exsit != null)
            {
                if (await userManager.IsInRoleAsync(exsit, Roles.Employee))
                {
                    var Reserves = _context.Reserved.Select(r => new ShowEmployeeReserveViewModel
                    {
                        CarId = r.CarId,
                        CarName = r.Car.Name,
                        ReserveId = r.ReservedId,
                        StartDate = r.StartDate,
                        EndDate = r.EndtDate,
                        ReserveType = r.ReserveType,
                        UserId = r.UserId,
                        Price = r.ReserveType == "minutes" ? r.Car.PricePerMinutes : (r.ReserveType == "hour" ? r.Car.PricePerHour : r.Car.PricePerDay)
                    }).ToList();
                    return Ok(Reserves);
                }
                else
                {
                    return Unauthorized("Not Employee");
                }
            }

            return Unauthorized();
        }

    }
}
