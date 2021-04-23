using System.Reflection;
using Domain.BaseModels;
using Domain.Models;
using Infrastructure.Persistence.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class DatabaseContext : IdentityDbContext<BaseUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> option) : base(option)
        {
        }

        public DbSet<Student> Students { get; set; }
        
        public DbSet<Instructor> Instructors { get; set; }
        
        public DbSet<Course> Courses { get; set; }

        public DbSet<BaseUser> BaseUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new BaseUserConfiguration());
            builder.ApplyConfiguration(new CourseConfiguration());
            builder.ApplyConfiguration(new EventConfiguration());
            builder.ApplyConfiguration(new InstructorConfiguration());
            builder.ApplyConfiguration(new StudentConfiguration());
            builder.ApplyConfiguration(new TimeConfiguration());

        }
    }
}