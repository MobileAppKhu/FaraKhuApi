using MediatR;

namespace Application.Features.Offer.Commands.DeleteOffer
{
    public class DeleteOfferCommand : IRequest<Unit>
    {
        public string OfferId { get; set; }
    }
}