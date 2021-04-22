using System.Linq;
using Application.DTOs.BaseUser;
using Application.DTOs.Instructor;
using Application.DTOs.Student;

namespace Application.Features.Account.SignUp
{
    public class SignUpViewModel
    {
        public IQueryable<BaseUserProfileDto> ProfileDto { get; set; }
    }
}