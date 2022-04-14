using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Event.PersonalEvent;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Event.Queries.SearchEvent
{
    public class SearchEventQueryHandler : IRequestHandler<SearchEventQuery, SearchEventViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IDatabaseContext _context;

        public SearchEventQueryHandler(IMapper mapper
            , IDatabaseContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<SearchEventViewModel> Handle(SearchEventQuery request, CancellationToken cancellationToken)
        {
            
            IQueryable<Domain.Models.Event> eventsQueryable = _context.Events.Where(e => e.UserId == request.UserId);

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                eventsQueryable = eventsQueryable.Where(e => e.EventDescription.Contains(request.Description));
            }

            if (!string.IsNullOrWhiteSpace(request.EventName))
            {
                eventsQueryable = eventsQueryable.Where(e => e.EventName.Contains(request.EventName));
            }

            if (!string.IsNullOrWhiteSpace(request.EventId))
            {
                eventsQueryable = eventsQueryable.Where(e => e.EventId == request.EventId);
            }

            if (request.EventTime != null)
            {
                eventsQueryable = eventsQueryable.Where(e => e.EventTime.Date.CompareTo(((DateTime) request.EventTime).Date) == 0);
            }

            if (!string.IsNullOrWhiteSpace(request.CourseId))
            {
                eventsQueryable = eventsQueryable.Where(e => e.CourseId == request.CourseId);
            }

            switch (request.EventColumn)
            {
                case EventColumn.EventId:
                    eventsQueryable = request.OrderDirection
                        ? eventsQueryable.OrderBy(e => e.EventId)
                        : eventsQueryable.OrderByDescending(e => e.EventId);
                    break;
                case EventColumn.EventDescription:
                    eventsQueryable = request.OrderDirection
                        ? eventsQueryable.OrderBy(e => e.EventDescription)
                            .ThenBy(e => e.EventId)
                        : eventsQueryable.OrderByDescending(e => e.EventDescription)
                            .ThenByDescending(e => e.EventId);
                    break;
                case EventColumn.EventName:
                    eventsQueryable = request.OrderDirection
                        ? eventsQueryable.OrderBy(e => e.EventName)
                            .ThenBy(e => e.EventId)
                        : eventsQueryable.OrderByDescending(e => e.EventName)
                            .ThenByDescending(e => e.EventId);
                    break;
                case EventColumn.EventTime:
                    eventsQueryable = request.OrderDirection
                        ? eventsQueryable.OrderBy(e => e.EventTime)
                            .ThenBy(e => e.EventId)
                        : eventsQueryable.OrderByDescending(e => e.EventTime)
                            .ThenByDescending(e => e.EventId);
                    break;
                case EventColumn.CreationDate:
                    eventsQueryable = request.OrderDirection
                        ? eventsQueryable.OrderBy(e => e.CreatedDate)
                            .ThenBy(e => e.EventId)
                        : eventsQueryable.OrderByDescending(e => e.CreatedDate).ThenByDescending(e => e.EventId);
                    break;
            }

            int searchLength = await eventsQueryable.CountAsync(cancellationToken);

            List<Domain.Models.Event> events = await eventsQueryable.Skip(request.Start).Take(request.Step)
                .ToListAsync(cancellationToken);
            return new SearchEventViewModel
            {
                Event = _mapper.Map<List<EventShortDto>>(events),
                SearchLength = searchLength
            };
        }
    }
}