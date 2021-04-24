using System.Linq;
using Application.Common.Interfaces.IServices;
using Domain.BaseModels;
using Domain.Models;
using Infrastructure.Persistence;

namespace Infrastructure.Services
{
    
    public class UserServices : IUserServices
    {
        private readonly DatabaseContext _context;

        public UserServices(DatabaseContext context)
        {
            _context = context;
        }
        public BaseUser GetUser(string id)
        {
            return _context.BaseUsers.FirstOrDefault(u => u.Id == id);
        }

        public Student GetStudentUser(string id)
        {
            return _context.Students.FirstOrDefault(u => u.Id == id);

        }

        public Instructor GetInstructorUser(string id)
        {
            return _context.Instructors.FirstOrDefault(u => u.Id == id);

        }
    }
}