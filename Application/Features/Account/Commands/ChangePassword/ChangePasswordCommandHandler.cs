using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private IStringLocalizer<SharedResource> Localizer { get; }
        private UserManager<BaseUser> UserManager { get; }
        private readonly IDatabaseContext _context;
        private IHttpContextAccessor HttpContextAccessor { get; }

        public ChangePasswordCommandHandler(UserManager<BaseUser> userManager,
            IStringLocalizer<SharedResource> localizer, IDatabaseContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            Localizer = localizer;
            UserManager = userManager;
            _context = context;
            HttpContextAccessor = httpContextAccessor;

        }
        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            BaseUser user = await UserManager.FindByIdAsync(userId);
            if(user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            if(UserManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash,
                request.OldPassword) == PasswordVerificationResult.Failed)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.InvalidPassword,
                    Message = Localizer["InvalidPassword"]
                });
            
            await UserManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}