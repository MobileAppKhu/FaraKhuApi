using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.User;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Queries.SearchAllEvents
{
    public class SearchAllEventsQueryHandler : IRequestHandler<SearchAllEventsQuery, SearchAllEventsViewModel>
    {
        private readonly IMapper _mapper;
        public UserManager<BaseUser> UserManager { get; }

        public IStringLocalizer<SharedResource> Localizer { get; }
        
        private IHttpContextAccessor HttpContextAccessor { get; }

        private readonly IDatabaseContext _context;

        public SearchAllEventsQueryHandler(IMapper mapper, UserManager<BaseUser> userManager,
            IStringLocalizer<SharedResource> localizer, IHttpContextAccessor httpContextAccessor
            , IDatabaseContext context)
        {
            _mapper = mapper;
            UserManager = userManager;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _context = context;
        }
        
        public async Task<SearchAllEventsViewModel> Handle(SearchAllEventsQuery request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            ICollection<string> roles = await UserManager.GetRolesAsync(user);
            BaseUser baseUser;
            if (roles.First() == UserType.Student.ToString())
                baseUser = _context.Students.Include(s => s.Events).
                    Include(s => s.Courses).ThenInclude(c => c.Times).
                    Include(s => s.Courses).ThenInclude(c => c.CourseEvents).
                    Include(s => s.Courses).ThenInclude(c => c.Instructor).
                    FirstOrDefault(s => s.Id == userId);
            else
                baseUser = _context.Instructors.Include(s => s.Events).
                    Include(i => i.Courses).ThenInclude(c => c.Times).
                    Include(i => i.Courses).ThenInclude(c => c.CourseEvents).
                    Include(i => i.Courses).ThenInclude(c => c.Instructor).
                    FirstOrDefault(s => s.Id == userId);

            return new SearchAllEventsViewModel
            {
                Events = _mapper.Map<AllEventsDto>(baseUser)
            };
        }
    }
}