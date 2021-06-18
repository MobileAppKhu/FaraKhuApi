using System.Collections.Generic;
using Application.DTOs.Offer;

namespace Application.Features.Offer.Query.ViewOffers
{
    public class ViewOffersViewModel
    {
        public ICollection<UserOfferDto> Offer { get; set; }
        public int SearchLength { get; set; }
    }
}