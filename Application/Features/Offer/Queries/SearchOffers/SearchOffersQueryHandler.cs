using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.Offer;
using AutoMapper;
using Domain.Enum;
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

            if (!string.IsNullOrWhiteSpace(request.User))
            {
                offerQueryable = offerQueryable.Where(offer => offer.UserId == request.User);
            }

            if (!string.IsNullOrWhiteSpace(request.StartPrice))
            {
                offerQueryable = offerQueryable.Where(offer => Convert.ToInt64(offer.Price) >= Convert.ToInt64(request.StartPrice));
            }

            if (!string.IsNullOrWhiteSpace(request.EndPrice))
            {
                offerQueryable = offerQueryable.Where(offer =>
                    Convert.ToInt64(offer.Price) <= Convert.ToInt64(request.EndPrice));
            }

            switch (request.OfferColumn)
            {
                case OfferColumn.OfferId:
                    offerQueryable = request.OrderDirection
                        ? offerQueryable.OrderBy(offer => offer.OfferId)
                        : offerQueryable.OrderByDescending(offer => offer.OfferId);
                    break;
                case OfferColumn.Title:
                    offerQueryable = request.OrderDirection
                        ? offerQueryable.OrderBy(offer => offer.Title).ThenBy(offer => offer.OfferId) 
                        : offerQueryable.OrderByDescending(offer => offer.Title).ThenByDescending(offer => offer.AvatarId);
                    break;
                case OfferColumn.Description:
                    offerQueryable = request.OrderDirection
                        ? offerQueryable.OrderBy(offer => offer.Description).ThenBy(offer => offer.OfferId)
                        : offerQueryable.OrderByDescending(offer => offer.Description)
                            .ThenByDescending(offer => offer.OfferId);
                    break;
                case OfferColumn.OfferType:
                    offerQueryable = request.OrderDirection
                        ? offerQueryable.OrderBy(offer => offer.OfferType).ThenBy(offer => offer.OfferId)
                        : offerQueryable.OrderByDescending(offer => offer.OfferType)
                            .ThenByDescending(offer => offer.OfferId);
                    break;
                case OfferColumn.Price:
                    offerQueryable = request.OrderDirection
                        ? offerQueryable.OrderBy(offer => offer.Price).ThenBy(offer => offer.OfferId)
                        : offerQueryable.OrderByDescending(offer => offer.Price)
                            .ThenByDescending(offer => offer.OfferId);
                    break;
                case OfferColumn.CreationDate:
                    offerQueryable = request.OrderDirection
                        ? offerQueryable.OrderBy(offer => offer.CreatedDate)
                            .ThenBy(offer => offer.OfferId)
                        : offerQueryable.OrderByDescending(offer => offer.CreatedDate)
                            .ThenByDescending(offer => offer.OfferId);
                    break;
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