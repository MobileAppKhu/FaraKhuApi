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

namespace Application.Features.Offer.Commands.DeleteOffer
{
    public class DeleteOfferCommandHandler : IRequestHandler<DeleteOfferCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }

        public DeleteOfferCommandHandler( IStringLocalizer<SharedResource> localizer,
            IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
        }
        public async Task<Unit> Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
        {
            BaseUser user = await _context.BaseUsers
                .Include(u => u.Offers).FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
                
            Domain.Models.Offer offer = await _context.Offers
                .Include(o => o.Avatar)
                .Include(o => o.BaseUser)
                .FirstOrDefaultAsync(o => o.OfferId == request.OfferId, cancellationToken);
            if (offer == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.OfferNotFound,
                    Message = Localizer["OfferNotFound"]
                });
            }
            
            if (offer.BaseUser != user && user.UserType != UserType.Owner)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                }); 
            }
            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}