using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
{
    private IStringLocalizer<SharedResource> Localizer { get; }
    private UserManager<BaseUser> UserManager { get; }
    private readonly IDatabaseContext _context;

    public ChangePasswordCommandHandler(UserManager<BaseUser> userManager,
        IStringLocalizer<SharedResource> localizer, IDatabaseContext context)
    {
        Localizer = localizer;
        UserManager = userManager;
        _context = context;

    }
    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        BaseUser user = await UserManager.FindByIdAsync(request.UserId);
        if (UserManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash,
                request.OldPassword) == PasswordVerificationResult.Failed)
        {
            throw new CustomException(new Error
            {
                ErrorType = ErrorType.InvalidPassword,
                Message = Localizer["InvalidPassword"]
            });
        }
                
        await UserManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}