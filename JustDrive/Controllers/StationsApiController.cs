using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JustDrive.Data;
using JustDrive.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JustDrive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsApiController : ControllerBase
    {

        private ApplicationDbContext db;
        private UserManager<User> userManager;
        public StationsApiController(ApplicationDbContext applicationDbContext , UserManager<User> user)
        {
            db = applicationDbContext;
            userManager = user;
        }

        public class StationsNameViewModel
        {
            public int StationNumber { get; set; }
            public string StationName { get; set; }
        }

        [HttpGet]
        public List<StationsNameViewModel> Get()
        {
            return db.Station.Select(s=>new StationsNameViewModel
            { 
                    StationNumber = s.StationNumber,
                    StationName = s.Name
            }).Distinct().ToList();
        }

        [HttpGet]
        [Route("StationViewDetails")]
        public List<StationViewModel> StationView(int id)
        {

            var st = db.Station.Where(s => s.StationNumber == id).Select(st => new StationViewModel { 
                EmpName = st.User.FirstName +" "+st.User.LastName,
                PhoneNumber = st.User.PhoneNumber,
                StationName = st.Name,
                Cars = st.Car            
            }).ToList();
         

            return st;
        }

        [HttpGet]
        [Route("carDetails")]
        public Car carDetails(int id)
        {
            var car = db.Car.FirstOrDefault(s => s.CarId == id);
            return car;
        }

        [HttpGet]
        [Route("StationSearch")]

        public Station StationSearch(string name)
        {
            var station = db.Station.FirstOrDefault(s => s.Name.ToLower().Equals(name.ToLower()));
            return station;
        }

        [HttpPost("upload")]
        public async Task<string> upload([FromForm]List<IFormFile> files)
        {
            try
            {
                var result = new List<FileUploadResult>();
                string path = "";
                foreach (var file in files)
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", file.FileName);
                    var stream = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(stream);
                    result.Add(new FileUploadResult() { Name = file.FileName, Length = file.Length });
                }
                return path;
            }
            catch
            {
                return "upload failed";
            }
        }




        public class EmployeeStation
        {
            public int StationNumber { get; set; }
            public string StationName { get; set; }
        }

        [HttpGet]
        [Route("EmployeeStations")]

        public async Task<IActionResult> EmployeeStations(string EmpId)
        {
            var Emp = await userManager.FindByIdAsync(EmpId);      
            if(Emp == null)
            {
                return BadRequest("Invalid Id");
            }
          
            if (await userManager.IsInRoleAsync(Emp, "Employee"))
            {
                var Station = db.Station.FirstOrDefault(s => s.UserId.Equals(EmpId));
                var ES = new EmployeeStation
                {
                    StationNumber = Station.StationNumber,
                    StationName = Station.Name
                };
                return Ok(ES);
            }
            else
            {
                return BadRequest();
            }
        }



        //return station info
    }
}