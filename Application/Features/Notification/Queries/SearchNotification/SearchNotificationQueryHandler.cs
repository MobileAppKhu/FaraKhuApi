using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.Notification;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Notification.Queries.SearchNotification;

public class SearchNotificationQueryHandler : IRequestHandler<SearchNotificationQuery, SearchNotificationViewModel>
{
    private readonly IDatabaseContext _context;
    private IMapper Mapper { get; }
        
    public SearchNotificationQueryHandler(IMapper mapper, IDatabaseContext context)
    {
        _context = context;
        Mapper = mapper;
    }
        
    public async Task<SearchNotificationViewModel> Handle(SearchNotificationQuery request, CancellationToken cancellationToken)
    {

        var notifications = await _context.Notifications
            .Where(notification => notification.UserId == request.UserId).ToListAsync(cancellationToken);

        return new SearchNotificationViewModel
        {
            Notifications = Mapper.Map<List<NotificationSearchDto>>(notifications)
        };
    }
}