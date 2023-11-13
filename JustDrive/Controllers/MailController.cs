using JustDrive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustDrive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService mailService;
        public static  string c = "";
        private UserManager<User> userManager;
        public MailController(IMailService service , UserManager<User> user)
        {
            this.mailService = service;
            userManager = user;
        }
         
        public string Random_Gen()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }
        [HttpGet("send")]
        [AllowAnonymous]
        public async Task<string> SendMail(string mail)
        {
            try
            {
                c = Random_Gen();
                MailRequest mailRequest = new MailRequest
                {
                    Subject = "Just Drive Forget Code",
                    Body = c,
                    Code = c,
                    ToEmail = mail,
                };

                    await mailService.SendEmailAsync(mailRequest);
                    return c;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }
  
        [HttpPost("isvalid")]
        public string isvalid(string a)
        {
            if(c == a)
            {
                return "Valid" ;
            }
            else
            {
                return "invalid";
            }
        }

        [HttpPost("change")]
        public async Task<int> changePassword(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            var user = await userManager.FindByIdAsync(forgetPasswordViewModel.UserId);
            if(user != null)
            {
                string tk = await userManager.GeneratePasswordResetTokenAsync(user);

                var new_pass = await userManager.ResetPasswordAsync(user, tk, forgetPasswordViewModel.Password);
                if (new_pass.Succeeded)
                {
                    return 1;
                }
            }
            return 0;
        }


    }
}
