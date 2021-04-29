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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private IStringLocalizer<SharedResource> Localizer { get; }
        private UserManager<BaseUser> UserManager { get; }
        private IEmailService _emailService { get; }
        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper;
        public ResetPasswordCommandHandler(UserManager<BaseUser> userManager,
            IStringLocalizer<SharedResource> localizer, IEmailService emailService, 
            IDatabaseContext context, IMapper mapper)
        {
            Localizer = localizer;
            UserManager = userManager;
            _emailService = emailService;
            _context = context;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await UserManager.FindByEmailAsync(request.Email.EmailNormalize());
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.UserNotFound, //TODO Email not found
                    Message = "Email Not Found" //TODO
                });
            
            if(!user.ResettingPassword)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unexpected, //TODO NotResetting
                    Message = "Not Resetting" //TODO
                });
            user.ResettingPassword = false;
            await UserManager.RemovePasswordAsync(user);
            await UserManager.AddPasswordAsync(user, request.NewPassword);
            _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}