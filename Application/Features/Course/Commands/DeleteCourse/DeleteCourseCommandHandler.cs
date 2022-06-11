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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Course.Commands.DeleteCourse
{
    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }

        public DeleteCourseCommandHandler(IStringLocalizer<SharedResource> localizer, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
        }

        public async Task<Unit> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            Instructor user = _context.Instructors.FirstOrDefault(u => u.Id == request.UserId);
            var courseObj = _context.Courses.Include(c => c.Students)
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

            _context.Courses.Remove(courseObj);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}