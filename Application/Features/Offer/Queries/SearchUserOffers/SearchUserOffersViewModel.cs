using System.Collections.Generic;
using Application.DTOs.Offer;

namespace Application.Features.Offer.Queries.SearchUserOffers
{
    public class SearchUserOffersViewModel
    {
        public ICollection<OfferDto> Offers { get; set; }
    }
}