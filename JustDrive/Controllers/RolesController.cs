using JustDrive.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustDrive.Controllers
{
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> roleManager;
        UserManager<User> userManager;
        public RolesController(RoleManager<IdentityRole> r , UserManager<User> u)
        {
            roleManager = r;
            userManager = u;
        }
        public IActionResult Index()
        {
            var Roles = roleManager.Roles;
            return View(Roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create (RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole(roleViewModel.RoleName);
                var role = await roleManager.CreateAsync(identityRole);
                if(role.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(roleViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            var Role = await roleManager.FindByIdAsync(Id);
            if(Role == null)
            {
                return NotFound();
            }

            RoleViewModel roleViewModel = new RoleViewModel
            {
                RoleName = Role.Name
            };

            return View(roleViewModel);
        }

        [HttpGet]
        public async Task <IActionResult> AddRemove(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);
            var userRoleViewModel = new List<UserRoleViewModel>();
            
            foreach(var i in userManager.Users.ToList())
            {
                var u = new UserRoleViewModel
                {
                    UserId = i.Id,
                    UserName = i.UserName,
                    RoleId = role.Id
                };
                if(await userManager.IsInRoleAsync(i,role.Name))
                {
                    u.IsSelected = true;
                }
                else
                {
                    u.IsSelected = false;
                }
                userRoleViewModel.Add(u);
            }
            return View(userRoleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRemove(List<UserRoleViewModel> userRoleViewModel)
        {
            foreach (var i in userRoleViewModel) 
            {
                var role = await roleManager.FindByIdAsync(i.RoleId);
                var user = await userManager.FindByIdAsync(i.UserId);
                IdentityResult result = null;

                if (i.IsSelected)
                {

                    if (!(await userManager.IsInRoleAsync(user, role.Name)))
                    {
                        result = await userManager.AddToRoleAsync(user, role.Name);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        continue;
                    }
                }
                else if (!i.IsSelected)
                { 
                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        result = await userManager.RemoveFromRoleAsync(user, role.Name);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        continue;
                    }
                }
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (RoleViewModel roleViewModel)
        {
            if(ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(roleViewModel.Id);
                role.Name = roleViewModel.RoleName;
                var res = await roleManager.UpdateAsync(role);
                if(res.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(roleViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);

            if (role != null)
            {
                var DeletedRole = new RoleViewModel
                {
                    Id = role.Id,
                    RoleName = role.Name
                };
                return View(DeletedRole);
            }
            return NotFound();
        }

        public async Task<IActionResult> Delete(RoleViewModel roleViewModel)
        {
            var Deleted = await roleManager.FindByIdAsync(roleViewModel.Id);
            if (Deleted != null)
            {
                var result = await roleManager.DeleteAsync(Deleted);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(roleViewModel);
        }

        public async Task<IActionResult> Details(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);
            if(role ==null)
            {
                return NotFound();
            }
            return View(role);

        }

    }
}
