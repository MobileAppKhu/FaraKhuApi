using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Offer.Commands.EditOffer;

public class EditOfferCommandHandler : IRequestHandler<EditOfferCommand>
{
    private readonly IDatabaseContext _context;
    private IStringLocalizer<SharedResource> Localizer { get; }

    public EditOfferCommandHandler(IStringLocalizer<SharedResource> localizer
        , IDatabaseContext context)
    {
        _context = context;
        Localizer = localizer;
    }

    public async Task<Unit> Handle(EditOfferCommand request, CancellationToken cancellationToken)
    {
        BaseUser user = await _context.BaseUsers.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            

        var offerObj =
            await _context.Offers
                .Include(offer => offer.BaseUser)
                .FirstOrDefaultAsync(a => a.OfferId == request.OfferId, cancellationToken);
        if (offerObj == null)
        {
            throw new CustomException(new Error
                {ErrorType = ErrorType.OfferNotFound, Message = Localizer["OfferNotFound"]});
        }

        if (offerObj.BaseUser != user && user.UserType != UserType.Owner)
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