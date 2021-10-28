using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.Offer;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Offer.Queries.SearchOffers
{
    public class SearchOffersQueryHandler : IRequestHandler<SearchOffersQuery, SearchOffersViewModel>
    {
        private readonly IDatabaseContext _context;
        private IMapper _mapper { get; }

        public SearchOffersQueryHandler(IMapper mapper, IDatabaseContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SearchOffersViewModel> Handle(SearchOffersQuery request, CancellationToken cancellationToken)
        {
            List<Domain.Models.Offer> offers = _context.Offers.Include(offer => offer.BaseUser).ToList();
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                offers = offers.Where(offer => (offer.Title.ToLower().Contains(request.Search.ToLower()) ||
                                                offer.Description.ToLower().Contains(request.Search.ToLower()))).ToList();
            }

            if(request.OfferType != 0)
            {
                offers = offers.Where(offers => offers.OfferType == request.OfferType).ToList();
            }
            
            int searchLength = offers.Count;
            offers = offers.Skip(request.Start).Take(request.Step).ToList();
            return new SearchOffersViewModel
            {
                Offer = _mapper.Map<ICollection<UserOfferDto>>(offers),
                SearchLength = searchLength
            };
        }
    }
}