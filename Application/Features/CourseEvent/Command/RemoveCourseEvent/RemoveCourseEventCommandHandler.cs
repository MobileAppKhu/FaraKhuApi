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
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Application.Features.CourseEvent.Command.RemoveCourseEvent
{
    public class RemoveCourseEventCommandHandler : IRequestHandler<RemoveCourseEventCommand>
    {
        private readonly IDatabaseContext _context;
        
        private IStringLocalizer<SharedResource> Localizer { get; }
        
        private IHttpContextAccessor HttpContextAccessor { get; }
        
        private UserManager<BaseUser> UserManager { get; }
        private IMapper _mapper { get; }

        public RemoveCourseEventCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            UserManager = userManager;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(RemoveCourseEventCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Instructor user = _context.Instructors.FirstOrDefault(u => u.Id == userId);
            if(user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            var courseEventObj = _context.CourseEvents.
                FirstOrDefault(c => c.CourseEventId == request.CourseEventId);
            if(courseEventObj == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.CourseEventNotFound,
                    Message = Localizer["CourseEventNotFound"]
                });
            _context.CourseEvents.Remove(courseEventObj);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}