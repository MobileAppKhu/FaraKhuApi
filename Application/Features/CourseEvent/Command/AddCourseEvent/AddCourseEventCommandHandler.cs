using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.CourseEvent;
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

namespace Application.Features.CourseEvent.Command.AddCourseEvent
{
    public class AddCourseEventCommandHandler : IRequestHandler<AddCourseEventCommand, AddCourseEventViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IMapper _mapper { get; }

        public AddCourseEventCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<AddCourseEventViewModel> Handle(AddCourseEventCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Instructor user = _context.Instructors.FirstOrDefault(u => u.Id == userId);
            if(user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            Domain.Models.Course courseObj = _context.Courses.Include(c => c.CourseEvents).
                FirstOrDefault(c => c.CourseId == request.CourseId);
            if(courseObj == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.CourseNotFound,
                    Message = Localizer["CourseNotFound"]
                });
            Domain.Models.CourseEvent courseEvent = new Domain.Models.CourseEvent
            {
                EventName = request.EventName,
                EventDescription = request.EventDescription,
                EventType = request.EventType,
                Course = courseObj,
                CourseId = request.CourseId,
                EventTime = DateTimeOffset.Parse(request.EventTime).Date
            };
            await _context.CourseEvents.AddAsync(courseEvent, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new AddCourseEventViewModel
            {
                CourseEvent = _mapper.Map<ViewCourseCourseEventDto>(courseEvent)
            };
        }
    }
}