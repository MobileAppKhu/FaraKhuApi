using Domain.BaseModels;
using Domain.Models;

namespace Application.Common.Interfaces.IServices
{
    public interface IUserServices
    {
        public BaseUser GetUser(string id);
        public Student GetStudentUser(string id);
        public Instructor GetInstructorUser(string id);
        
    }
}