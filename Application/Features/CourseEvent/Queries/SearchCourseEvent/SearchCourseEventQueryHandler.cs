using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.CourseEvent;
using Application.Resources;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.CourseEvent.Queries.SearchCourseEvent
{
    public class SearchCourseEventQueryHandler : IRequestHandler<SearchCourseEventQuery, SearchCourseEventViewModel>
    {
        private readonly IDatabaseContext _context;
        private IMapper _mapper { get; }
        
        public SearchCourseEventQueryHandler(IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SearchCourseEventViewModel> Handle(SearchCourseEventQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Models.CourseEvent> courseEventsQueryable = _context.CourseEvents;
            if (!string.IsNullOrWhiteSpace(request.CourseEventId))
            {
                courseEventsQueryable = courseEventsQueryable.Where(ce => request.CourseEventId == ce.CourseEventId);
            }

            if (!string.IsNullOrWhiteSpace(request.EventName))
            {
                courseEventsQueryable = courseEventsQueryable.Where(ce => ce.EventName.Contains(request.EventName));
            }

            if (!string.IsNullOrWhiteSpace(request.EventDescription))
            {
                courseEventsQueryable =
                    courseEventsQueryable.Where(ce => ce.EventDescription.Contains(request.EventDescription));
            }

            if (!string.IsNullOrWhiteSpace(request.CourseId))
            {
                courseEventsQueryable = courseEventsQueryable.Where(ce => ce.CourseId == request.CourseId);
            }

            switch (request.CourseEventColumn)
            {
                case CourseEventColumn.CourseEventId:
                    courseEventsQueryable = request.OrderDirection
                        ? courseEventsQueryable.OrderBy(ce => ce.CourseEventId)
                        : courseEventsQueryable.OrderByDescending(ce => ce.CourseEventId);
                    break;
                case CourseEventColumn.CourseEventName:
                    courseEventsQueryable = request.OrderDirection
                        ? courseEventsQueryable.OrderBy(ce => ce.EventName)
                            .ThenBy(ce => ce.CourseEventId)
                        : courseEventsQueryable.OrderByDescending(ce => ce.EventName)
                            .ThenByDescending(ce => ce.CourseEventId);
                    break;
                case CourseEventColumn.CourseEventDescription:
                    courseEventsQueryable = request.OrderDirection
                        ? courseEventsQueryable.OrderBy(ce => ce.EventDescription)
                            .ThenBy(ce => ce.CourseEventId)
                        : courseEventsQueryable.OrderByDescending(ce => ce.EventDescription)
                            .ThenByDescending(ce => ce.CourseEventId);
                    break;
                case CourseEventColumn.CourseId:
                    courseEventsQueryable = request.OrderDirection
                        ? courseEventsQueryable.OrderBy(ce => ce.CourseId)
                            .ThenBy(ce => ce.CourseEventId)
                        : courseEventsQueryable.OrderByDescending(ce => ce.CourseId)
                            .ThenByDescending(ce => ce.CourseEventId);
                    break;
            }

            int searchLength = await courseEventsQueryable.CountAsync(cancellationToken);
            List<Domain.Models.CourseEvent> courseEvents = await courseEventsQueryable
                .Skip(request.Start)
                .Take(request.Step)
                .ToListAsync(cancellationToken);

            return new SearchCourseEventViewModel
            {
                CourseEvents = _mapper.Map<List<SearchCourseCourseEventDto>>(courseEvents),
                SearchLength = searchLength
            };
        }
    }
}