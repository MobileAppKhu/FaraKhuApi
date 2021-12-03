using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.Offer;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Offer.Queries.SearchOffers
{
    public class SearchOffersQueryHandler : IRequestHandler<SearchOffersQuery, SearchOffersViewModel>
    {
        private readonly IDatabaseContext _context;
        private IMapper Mapper { get; }

        public SearchOffersQueryHandler(IMapper mapper, IDatabaseContext context)
        {
            _context = context;
            Mapper = mapper;
        }

        public async Task<SearchOffersViewModel> Handle(SearchOffersQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Models.Offer> offerQueryable = _context.Offers.Include(offer => offer.BaseUser);
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                offerQueryable = offerQueryable.Where(offer =>
                    offer.Title.ToLower().Contains(request.Search.ToLower()) ||
                    offer.Description.ToLower().Contains(request.Search.ToLower()));
            }

            if (request.OfferType != 0)
            {
                offerQueryable = offerQueryable.Where(offer => offer.OfferType == request.OfferType);
            }

            int searchLength = await offerQueryable.CountAsync(cancellationToken);
            List<Domain.Models.Offer> offers = offerQueryable.Skip(request.Start).Take(request.Step).ToList();
            return new SearchOffersViewModel
            {
                Offer = Mapper.Map<ICollection<UserOfferDto>>(offers),
                SearchLength = searchLength
            };
        }
    }
}