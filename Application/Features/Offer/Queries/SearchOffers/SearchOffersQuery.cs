using Domain.Enum;
using MediatR;

namespace Application.Features.Offer.Queries.SearchOffers;

public class SearchOffersQuery : IRequest<SearchOffersViewModel>
{
    public OfferType OfferType { get; set; }
    public string Search { get; set; }
    public string User { get; set; }
    public string StartPrice { get; set; }
    public string EndPrice { get; set; }
    public int Start { get; set; }
    public int Step { get; set; }
    public OfferColumn OfferColumn { get; set; }
    public bool OrderDirection { get; set; }
}