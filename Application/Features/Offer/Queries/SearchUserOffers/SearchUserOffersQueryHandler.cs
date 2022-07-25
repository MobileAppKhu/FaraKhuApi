using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.Offer;
using AutoMapper;
using Domain.BaseModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Offer.Queries.SearchUserOffers;

public class SearchUserOffersQueryHandler : IRequestHandler<SearchUserOffersQuery, SearchUserOffersViewModel>
{
    private readonly IDatabaseContext _context;
    private IHttpContextAccessor HttpContextAccessor { get; }
    private IMapper _mapper { get; }

    public SearchUserOffersQueryHandler(IHttpContextAccessor httpContextAccessor, IMapper mapper
        , IDatabaseContext context)
    {
        _context = context;
        HttpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }
    public async Task<SearchUserOffersViewModel> Handle(SearchUserOffersQuery request, CancellationToken cancellationToken)
    {
        var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        BaseUser user = await _context.BaseUsers.Include(u => u.Offers).
            ThenInclude(o => o.Avatar).FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        return new SearchUserOffersViewModel
        {
            Offers = _mapper.Map<ICollection<OfferDto>>(user.Offers)
        };
    }
}