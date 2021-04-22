using Domain.BaseModels;
using Domain.Models;
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
    }
}