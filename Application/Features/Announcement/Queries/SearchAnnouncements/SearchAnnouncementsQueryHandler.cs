using Application.Common.Interfaces;
using Application.DTOs.Announcement;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enum;

namespace Application.Features.Announcement.Queries.SearchAnnouncements
{
    public class
        SearchAnnouncementsQueryHandler : IRequestHandler<SearchAnnouncementsQuery, SearchAnnouncementsViewModel>
    {
        private readonly IDatabaseContext _context;
        private IMapper _mapper { get; }

        public SearchAnnouncementsQueryHandler(IMapper mapper, IDatabaseContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SearchAnnouncementsViewModel> Handle(SearchAnnouncementsQuery request,
            CancellationToken cancellationToken)
        {

            IQueryable<Domain.Models.Announcement> announcementsQueryable = _context.Announcements
                .Include(announcement => announcement.BaseUser);

            if (!string.IsNullOrWhiteSpace(request.AnnouncementId))
            {
                announcementsQueryable = announcementsQueryable.Where(announcement =>
                    request.AnnouncementId == announcement.AnnouncementId);
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
                Announcements = _mapper.Map<List<SearchAnnouncementDto>>(announcements),
                SearchLength = searchLength
            };
        }
    }
}