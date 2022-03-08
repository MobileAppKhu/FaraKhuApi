using System.Linq;
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

namespace Application.Features.Account.Commands.SignIn
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, SignInViewModel>
    {
        private IMapper _mapper { get; }
        private UserManager<BaseUser> _userManager { get; }
        private IStringLocalizer<SharedResource> _localizer { get; }
        private SignInManager<BaseUser> _signInManager { get; }
        private readonly IDatabaseContext _context;
        private IEmailService _emailService { get; set; }

        public SignInCommandHandler(IMapper mapper, UserManager<BaseUser> userManager,
            SignInManager<BaseUser> signInManager, IStringLocalizer<SharedResource> localizer
            ,IDatabaseContext context, IEmailService emailService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
            _context = context;
            _emailService = emailService;
        }
        public async Task<SignInViewModel> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            // this api doesn't show favourites
            var user = await _userManager.FindByEmailAsync(request.Logon.EmailNormalize());
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.UserNotFound,
                    Message = _localizer["UserNotFound"]
                });
            if (!user.EmailConfirmed)
            {
                user.IsValidating = true;
                string validationCode = ConfirmEmailCodeGenerator.GenerateCode();
                user.ValidationCode = validationCode;

                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync(cancellationToken);
            
                _emailService.SendEmail(request.Logon,
                    "Farakhu", "EmailVerification", "EmailVerification",
                    validationCode);
                return null;
            }
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);
            
            if (!result.Succeeded)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.InvalidInput,
                    Message = _localizer["InvalidInput"]
                });
            var signInViewModel = new SignInViewModel
            {
                ProfileDto = _mapper.Map<ProfileDto>(user)
            };
            signInViewModel.ProfileDto.Roles = _userManager.GetRolesAsync(user).Result.ToArray();
            return signInViewModel;
        }
    }
}