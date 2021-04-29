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
        private IEmailService _emailService { get; }
        private readonly IDatabaseContext _context;

        public ResetPasswordValidationCommandHandler(UserManager<BaseUser> userManager,
            IStringLocalizer<SharedResource> localizer, IEmailService emailService, 
            IDatabaseContext context)
        {
            Localizer = localizer;
            UserManager = userManager;
            _emailService = emailService;
            _context = context;
        }
        public async Task<Unit> Handle(ResetPasswordValidationCommand request, CancellationToken cancellationToken)
        {
            var user = await UserManager.FindByEmailAsync(request.Email.EmailNormalize());
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.UserNotFound, //TODO Email not found
                    Message = "Email Not Found" //TODO
                });
            BaseUser baseUser =
                _context.BaseUsers.FirstOrDefault(u => u.Id == user.Id);
            if(!baseUser.IsValidating)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.UserNotFound, //TODO NotValidating
                    Message = "Not validating" //TODO
                });
            if (baseUser.ValidationCode.Normalize() == request.Token.Normalize())
            {
                baseUser.IsValidating = false;
                baseUser.ResettingPassword = true;
            }

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}