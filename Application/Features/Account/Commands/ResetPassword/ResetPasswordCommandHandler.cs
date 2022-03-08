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

namespace Application.Features.Account.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private IStringLocalizer<SharedResource> Localizer { get; }
        private UserManager<BaseUser> UserManager { get; }
        private IEmailService _emailService { get; }
        private readonly IDatabaseContext _context;
        
        public ResetPasswordCommandHandler(UserManager<BaseUser> userManager,
            IStringLocalizer<SharedResource> localizer, IEmailService emailService, 
            IDatabaseContext context)
        {
            Localizer = localizer;
            UserManager = userManager;
            _emailService = emailService;
            _context = context;
        }
        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await UserManager.FindByEmailAsync(request.Email.EmailNormalize());
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.EmailNotFound,
                    Message = Localizer["EmailNotFound"]
                });
            //TODO should check if new password doesn't match with the older one
            if(!user.ResettingPassword)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.UnauthorizedResetPassword,
                    Message = Localizer["UnauthorizedResetPassword"]
                });
            if(UserManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash,
                request.NewPassword) == PasswordVerificationResult.Success)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.DuplicatePassword,
                    Message = Localizer["DuplicatePassword"]
                });
            user.ResettingPassword = false;
            await UserManager.RemovePasswordAsync(user);
            await UserManager.AddPasswordAsync(user, request.NewPassword);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}