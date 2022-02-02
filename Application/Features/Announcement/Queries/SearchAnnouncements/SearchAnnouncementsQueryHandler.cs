using Application.Common.Interfaces;
using Application.DTOs.Announcement;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Domain.Enum;

namespace Application.Features.Announcement.Queries.SearchAnnouncements
{
    public class
        SearchAnnouncementsQueryHandler : IRequestHandler<SearchAnnouncementsQuery, SearchAnnouncementsViewModel>
    {
        private readonly IDatabaseContext _context;

        private IStringLocalizer<SharedResource> Localizer { get; }

        private IHttpContextAccessor HttpContextAccessor { get; }

        private UserManager<BaseUser> UserManager { get; }
        private IMapper _mapper { get; }

        public SearchAnnouncementsQueryHandler(IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            UserManager = userManager;
            _mapper = mapper;
        }

        public async Task<SearchAnnouncementsViewModel> Handle(SearchAnnouncementsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.BaseUsers.FirstOrDefaultAsync(baseUser => baseUser.Id == userId,
                cancellationToken);
            if (user == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            IQueryable<Domain.Models.Announcement> announcementsQueryable = _context.Announcements
                .Include(announcement => announcement.BaseUser)
                .Include(announcement => announcement.Department)
                .ThenInclude(department => department.Faculty);

            if (request.AnnouncementIds.Count != 0)
            {
                announcementsQueryable = announcementsQueryable.Where(announcement =>
                    request.AnnouncementIds.Contains(announcement.AnnouncementId));
            }

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                announcementsQueryable = announcementsQueryable.Where(announcement =>
                    announcement.AnnouncementDescription.Contains(request.Description));
            }

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                announcementsQueryable = announcementsQueryable.Where(announcement =>
                    announcement.AnnouncementTitle.Contains(request.Title));
            }

            if (!string.IsNullOrWhiteSpace(request.Department))
            {
                announcementsQueryable =
                    announcementsQueryable.Where(announcement => announcement.DepartmentId.Equals(request.Department));
            }

            if (!string.IsNullOrWhiteSpace(request.User))
            {
                announcementsQueryable =
                    announcementsQueryable.Where(announcement => announcement.UserId.Equals(request.User));
            }

            switch (request.AnnouncementColumn)
            {
                case AnnouncementColumn.AnnouncementId:
                    announcementsQueryable = request.OrderDirection
                        ? announcementsQueryable.OrderBy(announcement => announcement.AnnouncementId)
                        : announcementsQueryable.OrderByDescending(announcement => announcement.AnnouncementId);
                    break;
                case AnnouncementColumn.Description:
                    announcementsQueryable = request.OrderDirection
                        ? announcementsQueryable.OrderBy(announcement => announcement.AnnouncementDescription)
                            .ThenBy(announcement => announcement.AnnouncementId)
                        : announcementsQueryable.OrderByDescending(announcement =>
                                announcement.AnnouncementDescription)
                            .ThenByDescending(announcement => announcement.AnnouncementId);
                    break;
                case AnnouncementColumn.Title:
                    announcementsQueryable = request.OrderDirection
                        ? announcementsQueryable.OrderBy(announcement => announcement.AnnouncementTitle)
                            .ThenBy(announcement => announcement.AnnouncementId)
                        : announcementsQueryable.OrderByDescending(announcement => announcement.AnnouncementTitle)
                            .ThenBy(announcement => announcement.AnnouncementId);
                    break;
                case AnnouncementColumn.DepartmentId:
                    announcementsQueryable = request.OrderDirection
                        ? announcementsQueryable.OrderBy(announcement => announcement.DepartmentId)
                            .ThenBy(announcement => announcement.AnnouncementId)
                        : announcementsQueryable.OrderByDescending(announcement => announcement.DepartmentId)
                            .ThenByDescending(announcement => announcement.AnnouncementId);
                    break;
                case AnnouncementColumn.UserId:
                    announcementsQueryable = request.OrderDirection
                        ? announcementsQueryable.OrderBy(announcement => announcement.UserId)
                            .ThenBy(announcement => announcement.AnnouncementId)
                        : announcementsQueryable.OrderByDescending(announcement => announcement.UserId)
                            .ThenByDescending(announcement => announcement.AnnouncementId);
                    break;
                case AnnouncementColumn.CreationDate:
                    announcementsQueryable = request.OrderDirection
                        ? announcementsQueryable.OrderBy(announcement => announcement.CreatedDate)
                            .ThenBy(announcement => announcement.AnnouncementId)
                        : announcementsQueryable.OrderByDescending(announcement => announcement.CreatedDate)
                            .ThenByDescending(announcement => announcement.CreatedDate);
                    break;
            }

            int searchLength = await announcementsQueryable.CountAsync(cancellationToken);

            List<Domain.Models.Announcement> announcements = await announcementsQueryable
                .Skip(request.Start)
                .Take(request.Step)
                .ToListAsync(cancellationToken);

            return new SearchAnnouncementsViewModel()
            {
                Announcements = _mapper.Map<ICollection<SearchAnnouncementDto>>(announcements),
                SearchLength = searchLength
            };
        }
    }
}