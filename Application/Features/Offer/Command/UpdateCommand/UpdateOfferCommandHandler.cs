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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Offer.Command.UpdateCommand
{
    public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private UserManager<BaseUser> UserManager { get; }

        public UpdateOfferCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            UserManager = userManager;
        }
        public async Task<Unit> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            BaseUser user = _context.BaseUsers.FirstOrDefault(u => u.Id == userId);
            if(user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            var avatarObj = await _context.Files.FirstOrDefaultAsync(a => a.Id == request.AvatarId,
                cancellationToken);
            if (avatarObj == null)
                throw new CustomException(new Error
                    {ErrorType = ErrorType.FileNotFound, Message = Localizer["FileNotFound"]});
            Domain.Models.Offer offer = new Domain.Models.Offer
            {
                OfferId = request.OfferId,
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                OfferType = request.OfferType,
                BaseUser = user,
                UserId = user.Id,
                Avatar = avatarObj,
                AvatarId = request.AvatarId
            };
            _context.Offers.Update(offer);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}