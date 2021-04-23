using System.Linq;
using Application.DTOs.BaseUser;
using Application.DTOs.Instructor;
using Application.DTOs.Student;
using Application.DTOs.User;
using Domain.BaseModels;

namespace Application.Features.Account.SignUp
{
    public class SignUpViewModel
    {
        public ProfileDto ProfileDto { get; set; }
    }
}