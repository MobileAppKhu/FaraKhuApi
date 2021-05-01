using Domain.Enum;
using MediatR;

namespace Application.Features.Offer.Command.CreateOffer
{
    public class CreateOfferCommand : IRequest<CreateOfferViewModel>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public OfferType OfferType { get; set; }
        public string Price { get; set; }
    }
}