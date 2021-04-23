using Application.Common.Mappings;
using Application.DTOs.BaseUser;

namespace Application.DTOs.Student
{
    public class StudentProfileDto : BaseUserProfileDto, IMapFrom<Domain.Models.Student>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string StudentId { get; set; }

        public string Email { get; set; }
    }
}