using Application.DTOs.BaseUser;

namespace Application.DTOs.Student
{
    public class StudentProfileDto : BaseUserProfileDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string StudentId { get; set; }

        public string Email { get; set; }
    }
}