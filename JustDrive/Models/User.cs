using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustDrive.Models
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string DriverCertificate { get; set; }

        public string ImageId { get; set; }

    }

    public class FileUploadResult
    {
        public long Length { get; set; }

        public string Name { get; set; }
    }

}
