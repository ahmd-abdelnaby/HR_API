
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HrAPI.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Certificates> Certificates { get; set; }
        public DbSet<Compensation> Compensations { get; set; }
        public DbSet<CV_Bank> CV_Bank { get; set; }
        public DbSet<Evaluation> Evaluation { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<Excuse> Excuses { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<MissionRequest> MissionRequests { get; set; }
        public DbSet<NeedsRequest> NeedsRequests { get; set; }
        public DbSet<Profession> Professions { get; set; }
    }


}
