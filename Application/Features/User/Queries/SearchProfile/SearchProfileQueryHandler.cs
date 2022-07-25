using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.User;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Queries.SearchProfile;

public class SearchProfileQueryHandler : IRequestHandler<SearchProfileQuery, SearchProfileViewModel>
{
    private readonly IMapper _mapper;
    public IStringLocalizer<SharedResource> Localizer { get; }
    private readonly IDatabaseContext _context;
    public SearchProfileQueryHandler(IMapper mapper, IStringLocalizer<SharedResource> localizer,
        IDatabaseContext context)
    {
        _mapper = mapper;
        Localizer = localizer;
        _context = context;
    }
    public async Task<SearchProfileViewModel> Handle(SearchProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.BaseUsers.
            Include(u => u.Favourites).FirstOrDefaultAsync(u => u.Id == request.UserId
                , cancellationToken);
        if (user == null)
        {
            throw new CustomException(new Error
            {
                ErrorType = ErrorType.UserNotFound,
                Message = Localizer["UserNotFound"]
            });
        }
                
        return new SearchProfileViewModel
        {
            Profile = _mapper.Map<ProfileDto>(user)
        };

    }
}