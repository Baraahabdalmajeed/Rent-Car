using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JustDrive.Models
{
    public class EditViewModel
    {
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string DriveCert { get; set; }
        public string ImageId { get; set; }

        public IFormFile PdfFile { get; set; }

        [Required]
        public string Phone { get; set; }

        public string ConfirmEmail { get; set; }


    }
}
