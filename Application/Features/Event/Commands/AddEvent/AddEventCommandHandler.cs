using System;
using System.Linq;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Event.Commands.AddEvent;

public class AddEventCommandHandler : IRequestHandler<AddEventCommand, AddEventViewModel>
{
    private readonly IDatabaseContext _context;
    private IStringLocalizer<SharedResource> Localizer { get; }
    private IMapper _mapper { get; }

    public AddEventCommandHandler(IStringLocalizer<SharedResource> localizer, IMapper mapper
        , IDatabaseContext context)
    {
        _context = context;
        Localizer = localizer;
        _mapper = mapper;
    }

    public async Task<AddEventViewModel> Handle(AddEventCommand request, CancellationToken cancellationToken)
    {
        BaseUser user = _context.BaseUsers.FirstOrDefault(u => u.Id == request.UserId);
           

        var eventObj = new Domain.Models.Event()
        {
            EventName = request.EventName,
            EventDescription = request.EventDescription,
            EventTime = request.EventTime,
            CourseId = request.CourseId,
            CourseTitle = "",
            User = user,
            UserId = user.Id,
            isDone = false,
            CreatedDate = DateTime.Now
        };
            
        if (!string.IsNullOrWhiteSpace(request.CourseId))
        {
            var courseObj =
                await _context.Courses.Include(course => course.CourseType).FirstOrDefaultAsync(
                    course => course.CourseId == request.CourseId,
                    cancellationToken);
            if (courseObj == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.CourseNotFound,
                    Message = Localizer["CourseNotFound"]
                });
            }

            eventObj.CourseTitle = courseObj.CourseType.CourseTypeTitle;
        }
        await _context.Events.AddAsync(eventObj, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return new AddEventViewModel
        {
            EventDto = _mapper.Map<EventShortDto>(eventObj)
        };
    }
}