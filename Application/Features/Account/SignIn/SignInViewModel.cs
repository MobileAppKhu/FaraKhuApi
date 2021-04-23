using Application.DTOs.BaseUser;
using Application.DTOs.User;
using Domain.BaseModels;

namespace Application.Features.Account.SignIn
{
    public class SignInViewModel
    {
        public ProfileDto ProfileDto { get; set; }
    }
}