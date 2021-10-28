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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Announcement.Queries.SearchAnnouncements
{
    public class SearchAnnouncementsQueryHandler : IRequestHandler<SearchAnnouncementsQuery, SearchAnnouncementsViewModel>
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
            var list = await _context.Announcements
                .Include(announce => announce.BaseUser).ToListAsync(cancellationToken);


            if (!string.IsNullOrEmpty(request.Faculty))
            {
                list = list.Where(announce => announce.Faculty == request.Faculty).ToList();
            }

            if (!string.IsNullOrEmpty(request.Department))
            {
                list = list.Where(announce => announce.Department == request.Department).ToList();
            }

            int searchLength = list.Count;
            list = list.Skip(request.Start).Take(request.Step).ToList();
            return new SearchAnnouncementsViewModel()
            {
                Announcements = _mapper.Map<ICollection<SearchAnnouncementDto>>(list),
                SearchLength = searchLength
            };
        }
    }
}