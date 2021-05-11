using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Course;
using Application.DTOs.User;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Course.Queries.ViewMyCourses
{
    public class ViewMyCoursesQueryHandler : IRequestHandler<ViewMyCoursesQuery, ViewMyCoursesViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IMapper _mapper { get; }

        public ViewMyCoursesQueryHandler(IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IMapper mapper, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<ViewMyCoursesViewModel> Handle(ViewMyCoursesQuery request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            BaseUser user;
            if (_context.Students.Find(userId) != null)
                user = await _context.Students.Include(u => u.Courses).
                    FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            else
                user = await _context.Instructors.Include(u => u.Courses).
                    FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });

            return new ViewMyCoursesViewModel
            {
                Courses = _mapper.Map<UserCoursesDto>(user) 
            };
        }
    }
}