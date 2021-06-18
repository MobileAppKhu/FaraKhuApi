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

namespace Application.Features.Offer.Query.ViewOffers
{
    public class ViewOffersQueryHandler : IRequestHandler<ViewOffersQuery, ViewOffersViewModel>
    {
        private readonly IDatabaseContext _context;
        private IMapper _mapper { get; }

        public ViewOffersQueryHandler(IMapper mapper, IDatabaseContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ViewOffersViewModel> Handle(ViewOffersQuery request, CancellationToken cancellationToken)
        {
            List<Domain.Models.Offer> offers;
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                offers = _context.Offers.Include(offers => offers.BaseUser)
                    .Where(offer => (offer.Title.ToLower().Contains(request.Search.ToLower()) ||
                                     offer.Description.ToLower().Contains(request.Search.ToLower()))
                                    && offer.OfferType == request.OfferType).ToList();
            }
            else
            {
                offers = _context.Offers.Include(offers => offers.BaseUser)
                    .Where(offers => offers.OfferType == request.OfferType).ToList();
            }

            int searchLength = offers.Count;
            offers = offers.Skip(request.Start).Take(request.Step).ToList();
            return new ViewOffersViewModel
            {
                Offer = _mapper.Map<ICollection<UserOfferDto>>(offers),
                SearchLength = searchLength
            };
        }
    }
}