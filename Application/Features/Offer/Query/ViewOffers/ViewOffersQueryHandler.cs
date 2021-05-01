﻿using System.Collections.Generic;
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
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private UserManager<BaseUser> UserManager { get; }
        private IMapper _mapper { get; }

        public ViewOffersQueryHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            UserManager = userManager;
            _mapper = mapper;
        }
        public async Task<ViewOffersViewModel> Handle(ViewOffersQuery request, CancellationToken cancellationToken)
        {
            var offers = _context.Offers.Include(o => o.BaseUser).ToList();
            if (!string.IsNullOrWhiteSpace(request.Search))
                offers = offers.Where(offer => offer.Title.ToLower().Contains(request.Search.ToLower()) || 
                                               offer.Description.ToLower().Contains(request.Search.ToLower()))
                                                                                                    .ToList();
            if (request.OfferType == OfferType.Buy || request.OfferType == OfferType.Sell)
                offers = offers.Where(offer => offer.OfferType == request.OfferType).ToList();
            return new ViewOffersViewModel
            {
                Offer = _mapper.Map<ICollection<UserOfferDto>>(offers)
            };
        }   
    }
}