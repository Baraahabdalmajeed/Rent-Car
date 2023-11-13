using System;
using System.Collections.Generic;
using System.Text;
using JustDrive.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JustDrive.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Station> Station { get; set; }
        public DbSet<Reserved> Reserved { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Points> Points { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Car> Car { get; set; }

    }
}
