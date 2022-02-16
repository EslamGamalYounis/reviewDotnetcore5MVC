using System;
using CourseMvcNet5Part1StartProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourseMvcNet5Part1StartProject.Data
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions ) : base(dbContextOptions)
        {
            
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<Product> Product { get; set; }  
    }
}
