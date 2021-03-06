using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Notification.Commands.AddCourseNotification;

public class AddCourseNotificationCommandHandler : IRequestHandler<AddCourseNotificationCommand, AddCourseNotificationViewModel>
{
    private readonly IDatabaseContext _context;
    private IStringLocalizer<SharedResource> Localizer { get; }

    public AddCourseNotificationCommandHandler( IStringLocalizer<SharedResource> localizer, IDatabaseContext context)
    {
        _context = context;
        Localizer = localizer;
    }
        
    public async Task<AddCourseNotificationViewModel> Handle(AddCourseNotificationCommand request, CancellationToken cancellationToken)
    {
        var user =
            await _context.BaseUsers.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

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

        if (user.UserType != UserType.Instructor && user.UserType != UserType.Owner)
        {
            throw new CustomException(new Error
            {
                ErrorType = ErrorType.Unauthorized,
                Message = Localizer["Unauthorized"]
            });
        }

        if (courseObj.InstructorId != user.Id)
        {
            throw new CustomException(new Error
            {
                ErrorType = ErrorType.Unauthorized,
                Message = Localizer["Unauthorized"]
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