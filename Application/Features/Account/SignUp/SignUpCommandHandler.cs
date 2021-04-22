using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Resources;
using Application.Utilities;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.SignUp
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand,SignUpViewModel>
    {
        private UserManager<BaseUser> _userManager { get; }
        
        private IStringLocalizer<SharedResource> _localizer { get; }
        
        private SignInManager<BaseUser> _signInManager { get; }

        public SignUpCommandHandler(UserManager<BaseUser> userManager, IStringLocalizer<SharedResource> localizer,
            SignInManager<BaseUser> signInManager)
        {
            _userManager = userManager;
            _localizer = localizer;
            _signInManager = signInManager;
        }
        public async Task<SignUpViewModel> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var duplicateUser = await _userManager.FindByEmailAsync(request.Email.EmailNormalize());
            if (duplicateUser != null)
            {
                throw new CustomException(new Error
                {
                    Message = _localizer.GetString(() => Resources_SharedResource_fa_IR.DuplicateUser),
                    ErrorType = ErrorType.DuplicateUser
                });
            }

            return null;
        }
    }

    
}