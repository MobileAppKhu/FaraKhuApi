using Application.Common.Mappings;
using Application.DTOs.BaseUser;
using AutoMapper;

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