using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JustDrive.Data;
using JustDrive.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace JustDrive.Controllers
{
    public class UsersManageController : Controller
    {
        UserManager<User> userManager;
        IPasswordHasher<User> passwordHasher;
        ApplicationDbContext _context;
        public UsersManageController(UserManager<User> user, IPasswordHasher<User> pass , ApplicationDbContext db)
        {
            userManager = user;
            passwordHasher = pass;
            _context = db;
        }
        public IActionResult Index()
        {
            var AllUsers = userManager.Users;
            return View(AllUsers);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User appUser = new User
                {
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName,
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Email,
                    PhoneNumber = registerViewModel.Phone,
                    ImageId = registerViewModel.ImageId,
                    DriverCertificate = registerViewModel.Password
                   
                };

                IdentityResult result = await userManager.CreateAsync(appUser, registerViewModel.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(registerViewModel);
        }


        public async Task<IActionResult> Details(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }


            var EditModel = new RegisterViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                Password = user.PasswordHash,
                ImageId = user.ImageId,
                
            };
            //https://localhost:44354/Identity/Account/ConfirmEmail?userId=32ea6fec-1084-4854-a661-fab82e1b601f&code=Q2ZESjhMZ01JY3JlVHNOQ29uOVJmNUpCSTJpT3dKVEhWbUpjQ1JkUzhEOENVSzdkZ3RQMzNycGpPNEczOVhBNlpSNTNJL0dyRWtvMHpRL2RhMU41RVBab3FaRUd1di9lQllGdFJIOHBhVnB1TVBHOWNaL3hHdXVSZ3FQYlJPWW9zRnRlb2Q4WjNrWm1RQnRLQnhRZjRCbmtVdFhrK3h4N3FuMmdlYWdBRmFmK1BCd2J2WHk5K01mM2loekZRMDZkRHBGMjh3M0plSnRSSWpNbjUvMkJGR0ZzdCt2bWdqTFBpUklSQjhNSjNqak5CbnhuNDArVnlYVjhkc1QrYm5tTnJJRmxzdz09

            //string code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            //var confirmationLink = Url.Action("ConfirmEmail", nameof(UsersManageController), new { userId = user.Id, code= code  }, Request.Scheme);
            ////// var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink, null);

            if (user.EmailConfirmed == false)
            {
                string confirmationToken = await userManager.
                     GenerateEmailConfirmationTokenAsync(user);

                string confirmationLink = Url.Action("ConfirmEmail", "UsersManage", new
                {
                    userId = user.Id,
                    token = confirmationToken,
                }, Request.Scheme);

                EditModel.EmailConfirmLink = confirmationLink;
                EditModel.EmailConfirmed = true;
            }
            return View(EditModel);

        }

        [HttpGet]

        public async Task<IActionResult> ConfirmEmail(string userId , string token)
        {
            if(userId == null || token == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return View("error");
            var result = await userManager.ConfirmEmailAsync(user, token);
           
            if(result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var uemail = await userManager.FindByIdAsync(registerViewModel.Id);
                uemail.FirstName = registerViewModel.FirstName;
                uemail.LastName = registerViewModel.LastName;
                uemail.Email = registerViewModel.Email;
                uemail.PasswordHash = passwordHasher.HashPassword(uemail, registerViewModel.Password);
                uemail.PhoneNumber = registerViewModel.Phone;
                uemail.ImageId = registerViewModel.ImageId;
                
                var isSucc = await userManager.UpdateAsync(uemail);
                if (isSucc.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in isSucc.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }

            return View(registerViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);

            if (user != null)
            {
                var DeletedUser = new RegisterViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };
                return View(DeletedUser);
            }

            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(RegisterViewModel registerViewModel)
        {
            var Deleted = await userManager.FindByIdAsync(registerViewModel.Id);
            if (Deleted != null)
            {
                var result = await userManager.DeleteAsync(Deleted);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(registerViewModel);
        }



    }
}