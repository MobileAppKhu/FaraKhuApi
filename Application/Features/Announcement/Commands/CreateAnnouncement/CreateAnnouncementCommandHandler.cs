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
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Announcement.Commands.CreateAnnouncement
{
    public class CreateAnnouncementCommandHandler : IRequestHandler<CreateAnnouncementCommand, CreateAnnouncementViewModel>
    {
        private readonly IDatabaseContext _context;

        private IStringLocalizer<SharedResource> Localizer { get; }

        private IHttpContextAccessor HttpContextAccessor { get; }

        
        private IMapper _mapper { get; }

        public CreateAnnouncementCommandHandler(IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<CreateAnnouncementViewModel> Handle(CreateAnnouncementCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            BaseUser user = _context.BaseUsers.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            Domain.Models.Announcement announcementObj = new Domain.Models.Announcement
            {
                AnnouncementTitle = request.Title,
                AnnouncementDescription = request.Description,
                Faculty = request.Faculty,
                Department = request.Department,
                BaseUser = user,
                UserId = userId
            };

            await _context.Announcements.AddAsync(announcementObj, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateAnnouncementViewModel
            {
                Announcement = _mapper.Map<ViewAnnouncementDto>(announcementObj)
            }; 
        }
    }
}
