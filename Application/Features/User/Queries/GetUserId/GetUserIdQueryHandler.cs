using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Queries.GetUserId
{
    public class GetUserIdQueryHandler : IRequestHandler<GetUserIdQuery, GetUserViewModel>
    {
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IDatabaseContext _context { get; }
        private IStringLocalizer<SharedResource> _localizer { get; }

        public GetUserIdQueryHandler(IHttpContextAccessor httpContextAccessor, IDatabaseContext context
        , IStringLocalizer<SharedResource> localizer)
        {
            HttpContextAccessor = httpContextAccessor;
            _context = context;
            _localizer = localizer;
        }
        public async Task<GetUserViewModel> Handle(GetUserIdQuery request, CancellationToken cancellationToken)
        {
            string userId = HttpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.BaseUsers.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.UserNotFound,
                    Message = _localizer["UserNotFound"]
                });
            return new GetUserViewModel
            {
                UserId = userId
            };
        }
    }
}