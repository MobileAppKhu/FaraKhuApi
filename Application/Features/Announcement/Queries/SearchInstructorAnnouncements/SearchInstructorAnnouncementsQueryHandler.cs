using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Announcement;
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

namespace Application.Features.Announcement.Queries.SearchInstructorAnnouncements
{
    public class SearchInstructorAnnouncementsQueryHandler : IRequestHandler<SearchInstructorAnnouncementsQuery,SearchInstructorAnnouncementsViewModel>
    {
        private readonly IDatabaseContext _context;
        
        private IStringLocalizer<SharedResource> Localizer { get; }
        
        private IHttpContextAccessor HttpContextAccessor { get; }
        
        private UserManager<BaseUser> UserManager { get; }
        private IMapper _mapper { get; }

        public SearchInstructorAnnouncementsQueryHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            UserManager = userManager;
            _mapper = mapper;
        }
        public async Task<SearchInstructorAnnouncementsViewModel> Handle(SearchInstructorAnnouncementsQuery request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            BaseUser user = await _context.BaseUsers.Include(baseUser => baseUser.Announcements).
                FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if(user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            var announcements = user.Announcements;
            int searchLength = announcements.Count;
            announcements = announcements.Skip(request.Start).Take(request.Step).ToList();
            return new SearchInstructorAnnouncementsViewModel
            {
                Announcements = _mapper.Map<ICollection<SearchAnnouncementDto>>(announcements),
                SearchLength = searchLength
            };
        }
    }
}