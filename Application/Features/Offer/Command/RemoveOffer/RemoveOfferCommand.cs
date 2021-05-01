using MediatR;

namespace Application.Features.Offer.Command.RemoveOffer
{
    public class RemoveOfferCommand : IRequest<Unit>
    {
        public int OfferId { get; set; }
    }
}