using System.Collections.Generic;
using Application.DTOs.Offer;

namespace Application.Features.Offer.Query.ViewUserOffers
{
    public class ViewUserOffersViewModel
    {
        public ICollection<OfferDto> Offers { get; set; }
    }
}