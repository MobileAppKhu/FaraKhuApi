﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.CourseEvent;
using Application.Features.Notification.SystemCallCommands;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.CourseEvent.Commands.AddCourseEvent
{
    public class AddCourseEventCommandHandler : IRequestHandler<AddCourseEventCommand, AddCourseEventViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IMapper Mapper { get; }

        public AddCourseEventCommandHandler(IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IMapper mapper, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            Mapper = mapper;
        }

        public async Task<AddCourseEventViewModel> Handle(AddCourseEventCommand request,
            CancellationToken cancellationToken)
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
            
            Domain.Models.Course courseObj = _context.Courses
                .Include(c => c.CourseEvents)
                .Include(c => c.Instructor)
                .Include(c => c. Students)
                .FirstOrDefault(c => c.CourseId == request.CourseId);
            
            if (courseObj == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.CourseNotFound,
                    Message = Localizer["CourseNotFound"]
                });
            }

            if (courseObj.Instructor != user && user.UserType != UserType.Owner)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            Domain.Models.CourseEvent courseEvent = new Domain.Models.CourseEvent
            {
                EventName = request.EventName,
                EventDescription = request.EventDescription,
                EventType = request.EventType,
                Course = courseObj,
                CourseId = request.CourseId,
                EventTime = request.EventTime
            };

            await _context.CourseEvents.AddAsync(courseEvent, cancellationToken);
            
            foreach (var student in courseObj.Students)
            {
                NotificationAdder.AddNotification(_context,
                    Localizer["YouHaveANewCourseEvent"],
                    courseEvent.CourseEventId, NotificationObjectType.CourseEvent,
                    NotificationOperation.NewCourseEvent, student);
            }
            
            await _context.SaveChangesAsync(cancellationToken);
            return new AddCourseEventViewModel
            {
                CourseEvent = Mapper.Map<SearchCourseCourseEventDto>(courseEvent)
            };
        }
    }
}