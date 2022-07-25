using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.User;
using AutoMapper;
using Domain.BaseModels;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Event.Queries.GetIncomingEvent;

public class GetIncomingEventQueryHandler : IRequestHandler<GetIncomingEventQuery, GetIncomingEventViewModel>
{
    private readonly IMapper _mapper;
    private readonly IDatabaseContext _context;

    public GetIncomingEventQueryHandler(IMapper mapper
        , IDatabaseContext context)
    {
        _mapper = mapper;
        _context = context;
    }
        
    public async Task<GetIncomingEventViewModel> Handle(GetIncomingEventQuery request, CancellationToken cancellationToken)
    {
        BaseUser baseUser = await _context.BaseUsers.Include(bu => bu.Events)
            .FirstOrDefaultAsync(bu => bu.Id == request.UserId, cancellationToken);
        baseUser.Events = baseUser.Events.Where(e => e.isDone == false && e.EventTime.CompareTo(DateTime.Now) > 0)
            .OrderBy(e => e.EventTime).Take(3).ToList();

        return new GetIncomingEventViewModel
        {
            Events = _mapper.Map<IncomingEventDto>(baseUser),
        };
    }
}