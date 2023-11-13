using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JustDrive.Data;
using JustDrive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JustDrive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private ApplicationDbContext db;
        private UserManager<User> userManager;
        private readonly IHostingEnvironment hostingEnvironment;

        IPasswordHasher<User> passwordHasher;
        public UsersController(ApplicationDbContext applicationDbContext, 
            UserManager<User> userManager,
              IPasswordHasher<User> passwordHasher, IHostingEnvironment hostingEnvironment
            )
        {
            db = applicationDbContext;
            this.userManager = userManager;
            this.passwordHasher = passwordHasher;
            this.hostingEnvironment = hostingEnvironment;
        }


        [HttpPost]
      
        [Route("Register")]
        public async Task<string> Register(RegisterViewModel registerViewModel)
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
                    DriverCertificate = registerViewModel.DriverCerticate,
                    ImageId = ""
                };

                IdentityResult result = await userManager.CreateAsync(appUser, registerViewModel.Password);

                if (result.Succeeded)
                {
                    return appUser.Id;
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        return error.Description;
                    }
                }
            }
            else 
                return "your Data is not valid";
            return "";
        }


        [HttpPost]
        [Route("Login")]

        public async Task<string> Login(LoginViewModel loginViewModel)
        {
           
             var user = await userManager.FindByEmailAsync(loginViewModel.Email);
            if (user == null)
            {
                return "Invalid Email";
            }
             
            else if (!await userManager.IsEmailConfirmedAsync(user))
            {
                return "Wait for Email Confirm";
            }
      
            var pass = await userManager.CheckPasswordAsync(user, loginViewModel.Password);
      
            if (pass == false)
            {
                return "Invalid pass";
            }

            else if (user != null && pass != false)
                {
                    return user.Id;
                }

            return"";
            
        }

        //-----------------------------------------------------------------------------
        [HttpPost]
        [Route("EmployeeLogin")]

        public async Task<string> EmployeeLogin(LoginViewModel loginViewModel)
        {

            var user = await userManager.FindByEmailAsync(loginViewModel.Email);

            if (user == null)
            {
                return "Invalid Email";
            }

            else if (!await userManager.IsEmailConfirmedAsync(user))
            {
                return "Wait for Email Confirm";
            }

            var pass = await userManager.CheckPasswordAsync(user, loginViewModel.Password);

            if (pass == false)
            {
                return "Invalid pass";
            }

            if(await userManager.IsInRoleAsync(user, Roles.Employee))
            {
                if (user != null && pass != false)
                {
                    return user.Id;
                }
            }

            return "";

        }
        //-----------------------------------------------------------------------------






        [HttpPost]
        [Route("EditProfile")]
        public async Task<int> Edit(EditViewModel model)
        {
            var IsEx = await userManager.FindByIdAsync(model.Id);
            if (IsEx != null)
            {
                var EditModel = new EditViewModel
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.Phone,
                };

                User u = await userManager.FindByIdAsync(EditModel.Id);

                u.FirstName = EditModel.FirstName;
                u.LastName = EditModel.LastName;
                u.PhoneNumber = EditModel.Phone;
                var IsDone = await userManager.UpdateAsync(u);
                if (IsDone.Succeeded)
                {
                    return 1;
                }
            }
            return 0;
        }

    }
}