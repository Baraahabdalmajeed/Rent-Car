using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JustDrive.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JustDrive.Controllers
{
    public class UserRolesController : Controller
    {
        private UserManager<User> userManager;

        public UserRolesController(UserManager<User> user)
        {
            userManager = user;
        }

        public async Task<IActionResult> Index()
        {
            var users = userManager.Users;
            List<User> Allusers = new List<User>();
            foreach (var i in users)
            {
                if (await userManager.IsInRoleAsync(i, "Admin"))
                {
                    Allusers.Add(i);
                }
            }
            return View(Allusers);
        }

    }
}