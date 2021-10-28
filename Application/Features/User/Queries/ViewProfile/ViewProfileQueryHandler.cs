using System.Linq;
using System.Security.Claims;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Queries.SearchProfile
{
    public class SearchProfileQueryHandler : IRequestHandler<SearchProfileQuery, SearchProfileViewModel>
    {
        private readonly IMapper _mapper;
        public IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private readonly IDatabaseContext _context;
        public SearchProfileQueryHandler(IMapper mapper, UserManager<BaseUser> userManager,
            IStringLocalizer<SharedResource> localizer, IHttpContextAccessor httpContextAccessor
            , IDatabaseContext context)
        {
            _mapper = mapper;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public async Task<SearchProfileViewModel> Handle(SearchProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.BaseUsers.
                Include(u => u.Favourites).FirstOrDefaultAsync(u => u.Id == userId
            , cancellationToken);
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            return new SearchProfileViewModel
            {
                Profile = _mapper.Map<ProfileDto>(user)
            };

        }
    }
}