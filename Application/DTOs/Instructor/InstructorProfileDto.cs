using Application.Common.Mappings;
using Application.DTOs.BaseUser;
using AutoMapper;

namespace Application.DTOs.Instructor
{
    public class InstructorProfileDto : BaseUserProfileDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string InstructorId { get; set; }

        public string Email { get; set; }
        
    }
}