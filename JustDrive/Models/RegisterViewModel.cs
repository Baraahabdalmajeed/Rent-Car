using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JustDrive.Models
{
    public class RegisterViewModel
    {
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public IFormFile PdfFile { get; set; }
        public string ImageId { get; set; }

        public string DriverCerticate { get; set; }


        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string Phone { get; set; }


        public bool EmailConfirmed { get; set; }

        public string EmailConfirmLink { get; set; }
    }
}
