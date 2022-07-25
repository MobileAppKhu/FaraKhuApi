using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.User;
using Application.Resources;
using Application.Utilities;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.Commands.EmailVerification;

public class EmailVerificationCommandHandler : IRequestHandler<EmailVerificationCommand, EmailVerificationViewModel>
{
    private IStringLocalizer<SharedResource> Localizer { get; }
    private UserManager<BaseUser> UserManager { get; }
    private readonly IDatabaseContext _context;
    private readonly IMapper _mapper;
    private SignInManager<BaseUser> _signInManager { get; }

    public EmailVerificationCommandHandler(UserManager<BaseUser> userManager,
        IStringLocalizer<SharedResource> localizer, IDatabaseContext context
        , IMapper mapper, SignInManager<BaseUser> signInManager)
    {
        Localizer = localizer;
        UserManager = userManager;
        _mapper = mapper;
        _context = context;
        _signInManager = signInManager;
    }
    public async Task<EmailVerificationViewModel> Handle(EmailVerificationCommand request, CancellationToken cancellationToken)
    {
        var user = await UserManager.FindByEmailAsync(request.Email.EmailNormalize());
        if (user == null)
            throw new CustomException(new Error
            {
                ErrorType = ErrorType.EmailNotFound,
                Message = Localizer["EmailNotFound"]
            });
        /*BaseUser baseUser =
            _context.BaseUsers.FirstOrDefault(u => u.Id == user.Id);*/
        if(!user.IsValidating)
            throw new CustomException(new Error
            {
                ErrorType = ErrorType.UnauthorizedValidation,
                Message = Localizer["UnauthorizedValidation"]
            });
        if (user.ValidationCode.Normalize() == request.Token.Normalize())
        {
            user.IsValidating = false;
            user.EmailConfirmed = true;
            await UserManager.UpdateAsync(user);
        }
        else
        {
            throw new CustomException(new Error
            {
                ErrorType = ErrorType.InvalidValidationToken,
                Message = Localizer["InvalidValidationToken"]
            });
        }
        if (user.EmailConfirmed)
        {
            await _signInManager.SignInAsync(user, false);
            await _context.SaveChangesAsync(cancellationToken);
            return new EmailVerificationViewModel
            {
                ProfileDto = _mapper.Map<ProfileDto>(user)
            };
        }
        return null;
    }
}