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

namespace Application.Features.Offer.Commands.EditOffer
{
    public class EditOfferCommandHandler : IRequestHandler<EditOfferCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private UserManager<BaseUser> UserManager { get; }

        public EditOfferCommandHandler(IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            UserManager = userManager;
        }

        public async Task<Unit> Handle(EditOfferCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            BaseUser user = _context.BaseUsers.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            var offerObj =
                await _context.Offers.Include(offer => offer.BaseUser).FirstOrDefaultAsync(a => a.OfferId == request.OfferId, cancellationToken);
            if (offerObj == null)
            {
                throw new CustomException(new Error
                    {ErrorType = ErrorType.OfferNotFound, Message = Localizer["OfferNotFound"]});
            }

            if (offerObj.BaseUser != user && user.UserType == UserType.Owner)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            if (!string.IsNullOrWhiteSpace(request.Price))
            {
                offerObj.Price = request.Price;
            }

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                offerObj.Title = request.Title;
            }

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                offerObj.Description = request.Description;
            }

            if (request.OfferType == OfferType.Buy || request.OfferType == OfferType.Sell)
            {
                offerObj.OfferType = request.OfferType;
            }

            if (!string.IsNullOrWhiteSpace(request.AvatarId))
            {
                var avatarObj = await _context.Files.FirstOrDefaultAsync(a => a.Id == request.AvatarId,
                    cancellationToken);
                if (avatarObj == null)
                    throw new CustomException(new Error
                        {ErrorType = ErrorType.FileNotFound, Message = Localizer["FileNotFound"]});
                offerObj.Avatar = avatarObj;
                offerObj.AvatarId = avatarObj.Id;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}