using Domain.Enum;
using MediatR;

namespace Application.Features.Offer.Query.ViewOffers
{
    public class ViewOffersQuery : IRequest<ViewOffersViewModel>
    {
        public OfferType OfferType { get; set; }
        public string Search { get; set; }
        public int Start { get; set; }
        public int Step { get; set; }
    }
}