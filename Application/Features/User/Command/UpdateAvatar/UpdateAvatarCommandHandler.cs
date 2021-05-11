using System.Linq;
using System.Security.Claims;
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
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Commands.UpdateAvatar
{
    public class UpdateAvatarCommandHandler : IRequestHandler<UpdateAvatarCommand, Unit>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private UserManager<BaseUser> UserManager { get; }
        private IMapper _mapper { get; }

        public UpdateAvatarCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            UserManager = userManager;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fileObj = _context.Files.FirstOrDefault(file => file.Id == request.FileId);
            var userObj =await UserManager.FindByIdAsync(userId);
            if (request.DeleteAvatar)
            {
                userObj.AvatarId = null;
                await UserManager.UpdateAsync(userObj);
                return Unit.Value;
            }
            if (fileObj == null)
                throw new CustomException(new Error
                    {ErrorType = ErrorType.FileNotFound, Message = Localizer["FileNotFound"]});
            userObj.Avatar = fileObj;
            await UserManager.UpdateAsync(userObj);
            return Unit.Value;
        }
    }
}