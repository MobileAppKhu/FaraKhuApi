using Domain.Enum;
using MediatR;

namespace Application.Features.Offer.Queries.SearchOffers
{
    public class SearchOffersQuery : IRequest<SearchOffersViewModel>
    {
        public OfferType OfferType { get; set; }
        public string Search { get; set; }
        public int Start { get; set; }
        public int Step { get; set; }
    }
}