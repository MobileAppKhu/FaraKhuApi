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
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            var courseObj = _context.Courses.Include(c => c.Students)
                .Include(c => c.CourseEvents)
                .Include(c => c.Times)
                .Include(c => c.Polls)
                .Include(c => c.Instructor).FirstOrDefault(c => c.CourseId == request.CourseId);
            
            return new SearchCourseViewModel
            {
                Course = _mapper.Map<SearchCourseDto>(courseObj)
            };
        }
    }
}