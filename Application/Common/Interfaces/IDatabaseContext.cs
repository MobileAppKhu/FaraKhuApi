using System.Threading;
using System.Threading.Tasks;
using Domain.BaseModels;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IDatabaseContext
    {
        public DbSet<Student> Students { get; set; }
        
        public DbSet<Instructor> Instructors { get; set; }
        
        public DbSet<Course> Courses { get; set; }

        public DbSet<BaseUser> BaseUsers { get; set; }

        public DbSet<Event> Events { get; set; }
        
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);


    }
}