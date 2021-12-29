using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Notification.Commands.AddCourseNotification
{
    public class AddCourseNotificationCommandHandler : IRequestHandler<AddCourseNotificationCommand, AddCourseNotificationViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }

        public AddCourseNotificationCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
        }
        
        public async Task<AddCourseNotificationViewModel> Handle(AddCourseNotificationCommand request, CancellationToken cancellationToken)
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

            if (user.UserType != UserType.Instructor && user.UserType != UserType.Owner)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            var courseObj = await _context.Courses
                .Include(course => course.Students)
                .FirstOrDefaultAsync(course => course.CourseId == request.CourseId, cancellationToken);
            if (courseObj == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.CourseNotFound,
                    Message = Localizer["CourseNotFound"]
                });
            }

            List<Domain.Models.Notification> notifications = new List<Domain.Models.Notification>();
            foreach (var student in courseObj.Students)
            {
                notifications.Add(new Domain.Models.Notification
                {
                    Description = request.Description,
                    EntityId = courseObj.CourseId,
                    NotificationOperation = NotificationOperation.InstructorMessage,
                    NotificationObjectType = NotificationObjectType.InstructorMessage,
                    User = student,
                    UserId = student.Id
                });
            }

            await _context.Notifications.AddRangeAsync(notifications, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new AddCourseNotificationViewModel();
        }
    }
}