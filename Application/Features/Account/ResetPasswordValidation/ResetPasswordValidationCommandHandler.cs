using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using Application.Utilities;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.ResetPasswordValidation
{
    public class ResetPasswordValidationCommandHandler : IRequestHandler<ResetPasswordValidationCommand>
    {
        private IStringLocalizer<SharedResource> Localizer { get; }
        private UserManager<BaseUser> UserManager { get; }

        private readonly IDatabaseContext _context;

        public ResetPasswordValidationCommandHandler(UserManager<BaseUser> userManager,
            IStringLocalizer<SharedResource> localizer, IDatabaseContext context)
        {
            Localizer = localizer;
            UserManager = userManager;
            
            _context = context;
        }
        public async Task<Unit> Handle(ResetPasswordValidationCommand request, CancellationToken cancellationToken)
        {
            var user = await UserManager.FindByEmailAsync(request.Email.EmailNormalize());
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.EmailNotFound,
                    Message = Localizer["EmailNotFound"]
                });
            BaseUser baseUser =
                _context.BaseUsers.FirstOrDefault(u => u.Id == user.Id);
            if(!baseUser.IsValidating)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.UnauthorizedValidation,
                    Message = Localizer["UnauthorizedValidation"]
                });
            if (baseUser.ValidationCode.Normalize() == request.Token.Normalize())
            {
                baseUser.IsValidating = false;
                baseUser.ResettingPassword = true;
            }else
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.InvalidValidationToken,
                    Message = Localizer["InvalidValidationToken"]
                });
            }

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}