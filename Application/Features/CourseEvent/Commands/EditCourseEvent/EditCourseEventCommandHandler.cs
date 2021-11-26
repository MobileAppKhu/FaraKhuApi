﻿using System;
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

namespace Application.Features.CourseEvent.Commands.EditCourseEvent
{
    public class EditCourseEventCommandHandler : IRequestHandler<EditCourseEventCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        public EditCourseEventCommandHandler(IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(EditCourseEventCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.BaseUsers.FirstOrDefaultAsync(baseUser => baseUser.Id == userId, cancellationToken);
            if (user == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            var editingCourseEvent =
                await _context.CourseEvents
                    .Include(courseEvent => courseEvent.Course)
                    .ThenInclude(course => course.Instructor)
                    .FirstOrDefaultAsync(
                    courseEvent => courseEvent.CourseEventId == request.CourseEventId, cancellationToken);
            if (editingCourseEvent == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.CourseEventNotFound,
                    Message = Localizer["CourseEventNotFound"]
                });
            }

            if (editingCourseEvent.Course.Instructor != user && user.UserType != UserType.Owner)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            if (!string.IsNullOrWhiteSpace(request.EventName))
            {
                editingCourseEvent.EventName = request.EventName;
            }

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                editingCourseEvent.EventDescription = request.Description;
            }

            if (request.CourseEventType != null)
            {
                editingCourseEvent.EventType = (CourseEventType)request.CourseEventType;
            }

            if (request.EventTime != null)
            {
                editingCourseEvent.EventTime = (DateTime) request.EventTime;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}