using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.User;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.User.Queries.SearchAllEvents;

public class SearchAllEventsQueryHandler : IRequestHandler<SearchAllEventsQuery, SearchAllEventsViewModel>
{
    private readonly IMapper _mapper;
    private readonly IDatabaseContext _context;

    public SearchAllEventsQueryHandler(IMapper mapper
        , IDatabaseContext context)
    {
        _mapper = mapper;
        _context = context;
    }
        
    public async Task<SearchAllEventsViewModel> Handle(SearchAllEventsQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.BaseUsers.FirstOrDefaultAsync(bu => bu.Id == request.UserId, cancellationToken);
        BaseUser baseUser;
        if (user.UserType == UserType.Student)
        {
            baseUser = await _context.Students.Include(s => s.Events).
                Include(s => s.Courses).ThenInclude(c => c.Times).
                Include(s => s.Courses).ThenInclude(c => c.CourseEvents).
                Include(s => s.Courses).ThenInclude(c => c.Instructor).
                Include(s => s.Courses).ThenInclude(c => c.CourseType).
                FirstOrDefaultAsync(s => s.Id == user.Id, cancellationToken);
        }
        else
        {
            baseUser = await _context.Instructors.Include(s => s.Events).
                Include(i => i.Courses).ThenInclude(c => c.Times).
                Include(i => i.Courses).ThenInclude(c => c.CourseEvents).
                Include(i => i.Courses).ThenInclude(c => c.Instructor).
                Include(i => i.Courses).ThenInclude(c => c.CourseType).
                FirstOrDefaultAsync(s => s.Id == user.Id, cancellationToken);
        }

        return new SearchAllEventsViewModel
        {
            Events = _mapper.Map<AllEventsDto>(baseUser)
        };
    }
}