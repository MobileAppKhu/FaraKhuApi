using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Course;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Application.Features.Course.Commands.AddCourse
{
    public class AddCourseCommandHandler : IRequestHandler<AddCourseCommand, AddCourseViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IMapper _mapper { get; }

        public AddCourseCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<AddCourseViewModel> Handle(AddCourseCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Instructor user = _context.Instructors.FirstOrDefault(u => u.Id == userId);
            if(user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            Domain.Models.Course courseObj = new Domain.Models.Course
            {
                CourseTitle = request.CourseTitle,
                Instructor = user,
                InstructorId = userId,
                EndDate = request.EndDate
            };
            await _context.Courses.AddAsync(courseObj, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new AddCourseViewModel
            {
                Course = _mapper.Map<SearchCourseDto>(courseObj)
            };
        }
    }
}