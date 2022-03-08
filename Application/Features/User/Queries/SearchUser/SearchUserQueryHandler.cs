using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Queries.SearchUser
{
    public class SearchUserQueryHandler : IRequestHandler<SearchUserQuery, SearchUserViewModel>
    {
        
        private readonly IMapper _mapper;
        public IStringLocalizer<SharedResource> Localizer { get; }
        private readonly IDatabaseContext _context;
        private IHttpContextAccessor HttpContextAccessor { get; }


        public SearchUserQueryHandler(IMapper mapper, IStringLocalizer<SharedResource> localizer
            , IDatabaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            Localizer = localizer;
            _context = context;
            HttpContextAccessor = httpContextAccessor;
        }
        public Task<SearchUserViewModel> Handle(SearchUserQuery request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            BaseUser user = _context.BaseUsers.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            if (user.UserType != UserType.Owner)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            return null;
        }
    }
}