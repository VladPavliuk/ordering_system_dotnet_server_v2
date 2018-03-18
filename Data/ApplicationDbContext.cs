using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mvc_auth.Models;

namespace mvc_auth.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // public DbSet<Admin> Admin { get; set; }
        // public DbSet<User> User { get; set; }
        public DbSet<Date> Date { get; set; }
        public DbSet<Organization> Organization { get; set; }
        public DbSet<OrganizationDateRelation> OrganizationDateRelation { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<OrganizationServiceRelation> OrganizationServiceRelation { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrganizationRating> OrganizationMarkup { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
