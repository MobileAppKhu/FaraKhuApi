using System.IO;
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
        public DbSet<Time> Times { get; set; }
        public DbSet<CourseEvent> CourseEvents { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Suggestion> Suggestions { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        public DbSet<PollQuestion> PollQuestions { get; set; }
        public DbSet<PollAnswer> PollAnswers { get; set; }
        public DbSet<FileEntity> Files { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
    }
}