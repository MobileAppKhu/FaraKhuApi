using System.Collections.Generic;
using Application.DTOs.Offer;

namespace Application.Features.Offer.Queries.SearchOffers
{
    public class SearchOffersViewModel
    {
        public ICollection<UserOfferDto> Offer { get; set; }
        public int SearchLength { get; set; }
    }
}