using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineEdu.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEdu.Data.EduContext
{
    public class EduDbContext : IdentityDbContext<IdentityUser>
    {
        public EduDbContext(DbContextOptions<EduDbContext> options) : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
