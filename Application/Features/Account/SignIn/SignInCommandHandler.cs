using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.DTOs.Instructor;
using Application.DTOs.Student;
using Application.DTOs.User;
using Application.Resources;
using Application.Utilities;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.SignIn
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, SignInViewModel>
    {
        private IMapper _mapper { get; }
        private UserManager<BaseUser> _userManager { get; }
        private IStringLocalizer<SharedResource> _localizer { get; }
        private SignInManager<BaseUser> _signInManager { get; }
        private DatabaseContext _context { get; }

        public SignInCommandHandler(IMapper mapper, UserManager<BaseUser> userManager,
            SignInManager<BaseUser> signInManager, IStringLocalizer<SharedResource> localizer
            , DatabaseContext context)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
            _context = context;
        }
        public async Task<SignInViewModel> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Logon.EmailNormalize());
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.UserNotFound,
                    Message = _localizer["UserNotFound"]
                });
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);
            
            if (!result.Succeeded)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.InvalidInput,
                    Message = _localizer["InvalidInput"]
                });

            return new SignInViewModel
            {
                ProfileDto = _mapper.Map<ProfileDto>(user)
            };
        }
    }
}