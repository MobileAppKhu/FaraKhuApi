using Application.Common.Mappings;
using Application.DTOs.BaseUser;

namespace Application.DTOs.Instructor
{
    public class InstructorProfileDto : BaseUserProfileDto, IMapFrom<Domain.Models.Instructor>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string InstructorId { get; set; }

        public string Email { get; set; }
    }
}