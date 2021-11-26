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

namespace Application.Features.CourseEvent.Commands.DeleteCourseEvent
{
    public class DeleteCourseEventCommandHandler : IRequestHandler<DeleteCourseEventCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        public DeleteCourseEventCommandHandler(IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(DeleteCourseEventCommand request, CancellationToken cancellationToken)
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
            
            var courseEventObj = _context.CourseEvents
                .Include(c => c.Course)
                .ThenInclude(course => course.Instructor)
                .FirstOrDefault(c => c.CourseEventId == request.CourseEventId);
            if(courseEventObj == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.CourseEventNotFound,
                    Message = Localizer["CourseEventNotFound"]
                });

            if (courseEventObj.Course.Instructor != user && user.UserType != UserType.Owner)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                }); 
            }
            
            _context.CourseEvents.Remove(courseEventObj);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}