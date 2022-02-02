using System;
using System.Collections.Generic;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Course.Queries.SearchCourse
{
    public class SearchCourseQueryHandler : IRequestHandler<SearchCourseQuery, SearchCourseViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IMapper _mapper { get; }

        public SearchCourseQueryHandler(IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IMapper mapper, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<SearchCourseViewModel> Handle(SearchCourseQuery request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            BaseUser user = await _context.BaseUsers.FirstOrDefaultAsync(u => u.Id == userId
            ,cancellationToken);
            if (user == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            IQueryable<Domain.Models.Course> coursesQueryable = _context.Courses
                .Include(course => course.Instructor)
                .Include(course => course.Students)
                .Include(course => course.Polls)
                .Include(course => course.Times)
                .Include(course => course.CourseType)
                .ThenInclude(type => type.Department)
                .ThenInclude(department => department.Faculty);

            if (!string.IsNullOrWhiteSpace(request.CourseId))
            {
                coursesQueryable = coursesQueryable.Where(course => course.CourseId == request.CourseId);
            }

            if (!string.IsNullOrWhiteSpace(request.CourseType))
            {
                coursesQueryable = coursesQueryable.Where(course => course.CourseTypeId == request.CourseType);
            }

            if (!string.IsNullOrWhiteSpace(request.Student))
            {
                var studentTemp =
                    await _context.Students.FirstOrDefaultAsync(student => student.Id == request.Student, cancellationToken);
                if (studentTemp == null)
                {
                    throw new CustomException(new Error
                    {
                        ErrorType = ErrorType.StudentNotFound,
                        Message = Localizer["StudentNotFound"]
                    });
                }
                coursesQueryable = coursesQueryable.Where(course =>
                    course.Students.Contains(studentTemp));
            }

            if (!string.IsNullOrWhiteSpace(request.Instructor))
            {
                coursesQueryable = coursesQueryable.Where(course => course.InstructorId == request.Instructor);
            }

            if (!string.IsNullOrWhiteSpace(request.CourseType))
            {
                coursesQueryable = coursesQueryable.Where(course => course.CourseTypeId == request.CourseType);
            }

            if (request.EndDate != null)
            {
                coursesQueryable = coursesQueryable.Where(course => course.EndDate == request.EndDate);
            }

            switch (request.CourseColumn)
            {
                case CourseColumn.CourseId:
                    coursesQueryable = request.OrderDirection
                        ? coursesQueryable.OrderBy(course => course.CourseId)
                        : coursesQueryable.OrderByDescending(course => course.CourseId);
                    break;
                case CourseColumn.CourseTypeId:
                    coursesQueryable = request.OrderDirection
                        ? coursesQueryable.OrderBy(course => course.CourseType)
                            .ThenBy(course => course.CourseId)
                        : coursesQueryable.OrderByDescending(course => course.CourseType)
                            .ThenByDescending(course => course.CourseId);
                    break;
                case CourseColumn.InstructorId:
                    coursesQueryable = request.OrderDirection
                        ? coursesQueryable.OrderBy(course => course.InstructorId)
                            .ThenBy(course => course.CourseId)
                        : coursesQueryable.OrderByDescending(course => course.InstructorId)
                            .ThenByDescending(course => course.CourseId);
                    break;
                case CourseColumn.EndDate:
                    coursesQueryable = request.OrderDirection
                        ? coursesQueryable.OrderBy(course => course.EndDate)
                            .ThenBy(course => course.EndDate)
                        : coursesQueryable.OrderByDescending(course => course.EndDate)
                            .ThenByDescending(course => course.CourseId);
                    break;
                case CourseColumn.CreationDate:
                    coursesQueryable = request.OrderDirection
                        ? coursesQueryable.OrderBy(course => course.CreatedDate)
                            .ThenBy(course => course.CourseId)
                        : coursesQueryable.OrderByDescending(course => course.CreatedDate)
                            .ThenByDescending(course => course.InstructorId);
                    break;
            }

            int searchLength = await coursesQueryable.CountAsync(cancellationToken);

            List<Domain.Models.Course> courses = await coursesQueryable
                .Skip(request.Start)
                .Take(request.Step)
                .ToListAsync(cancellationToken);
            
            return new SearchCourseViewModel
            {
                Course = _mapper.Map<List<SearchCourseDto>>(courses),
                SearchLength = searchLength
            };
        }
    }
}