using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IDatabaseContext _context;
        
        private IStringLocalizer<SharedResource> _localizer { get; }
        
        private IHttpContextAccessor HttpContextAccessor { get; }
        
        private UserManager<BaseUser> _userManager { get; }
        private IMapper _mapper { get; }

        public DeleteUserCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            _localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            // TODO
            var user = await _context.BaseUsers.FirstOrDefaultAsync(u => u.Id == request.UserId,
                                                                    cancellationToken);
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.UserNotFound,
                    Message = _localizer["UserNotFound"]
                });
            _context.BaseUsers.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}