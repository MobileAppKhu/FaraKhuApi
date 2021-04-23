using System.Linq;
using Application.DTOs.BaseUser;
using Application.DTOs.Instructor;
using Application.DTOs.Student;
using Domain.BaseModels;

namespace Application.Features.Account.SignUp
{
    public class SignUpViewModel
    {
        public BaseUserProfileDto ProfileDto { get; set; }
    }
}