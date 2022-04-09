using System;
using System.Linq;
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

namespace Application.Features.Event.Commands.EditEvent
{
    public class EditEventCommandHandler : IRequestHandler<EditEventCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }

        public EditEventCommandHandler(IStringLocalizer<SharedResource> localizer, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
        }
        public async Task<Unit> Handle(EditEventCommand request, CancellationToken cancellationToken)
        {
            BaseUser user = await _context.BaseUsers.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
                
            var eventObj = _context.Events
                .Include(e => e.User)
                .FirstOrDefault(e => e.EventId == request.EventId);
            if (eventObj == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.EventNotFound,
                    Message = Localizer["EventNotFound"]
                });
            }

            if (eventObj.User != user && user.UserType != UserType.Owner)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                }); 
            }
            
            if (!string.IsNullOrWhiteSpace(request.EventName))
            {
                eventObj.EventName = request.EventName;
            }
            if (!string.IsNullOrWhiteSpace(request.EventDescription))
            {
                eventObj.EventDescription = request.EventDescription;
            }
            if (!string.IsNullOrWhiteSpace(request.EventTime))
            {
                eventObj.EventTime = DateTimeOffset.Parse(request.EventTime).Date;
            }
            if (request.changingIsDone)
            {
                eventObj.isDone = !eventObj.isDone;
            }

            if (!string.IsNullOrWhiteSpace(request.CourseId))
            {
                var courseObj =
                    await _context.Courses.Include(course => course.CourseType).FirstOrDefaultAsync(
                        course => course.CourseId == request.CourseId,
                        cancellationToken);
                eventObj.CourseId = courseObj.CourseId;
                eventObj.CourseTitle = courseObj.CourseType.CourseTypeTitle;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}