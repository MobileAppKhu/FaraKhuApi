using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Notification;
using Application.Features.Ticket.Queries.SearchTicket;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Notification.Queries.SearchNotification
{
    public class SearchNotificationQueryHandler : IRequestHandler<SearchNotificationQuery, SearchNotificationViewModel>
    {
        private readonly IDatabaseContext _context;
        private IMapper Mapper { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IStringLocalizer<SharedResource> Localizer { get; }
        
        public SearchNotificationQueryHandler(IMapper mapper, IDatabaseContext context,
            IHttpContextAccessor httpContextAccessor, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            Mapper = mapper;
            HttpContextAccessor = httpContextAccessor;
            Localizer = localizer;
        }
        
        public async Task<SearchNotificationViewModel> Handle(SearchNotificationQuery request, CancellationToken cancellationToken)
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

            var notifications = await _context.Notifications
                .Where(notification => notification.UserId == userId).ToListAsync(cancellationToken);

            return new SearchNotificationViewModel
            {
                Notifications = Mapper.Map<List<NotificationSearchDto>>(notifications)
            };
        }
    }
}