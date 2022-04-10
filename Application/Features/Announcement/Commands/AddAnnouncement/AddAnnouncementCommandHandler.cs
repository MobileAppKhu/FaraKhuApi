using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Announcement;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Announcement.Commands.AddAnnouncement
{
    public class AddAnnouncementCommandHandler : IRequestHandler<AddAnnouncementCommand, AddAnnouncementViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IMapper Mapper { get; }

        public AddAnnouncementCommandHandler(IStringLocalizer<SharedResource> localizer,
            IMapper mapper, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            Mapper = mapper;
        }

        public async Task<AddAnnouncementViewModel> Handle(AddAnnouncementCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _context.BaseUsers.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (string.IsNullOrWhiteSpace(request.Avatar))
            {
                request.Avatar = "smiley.png";
            }

            var avatarObj =
                await _context.Files.FirstOrDefaultAsync(entity => entity.Id == request.Avatar, cancellationToken);
            if (avatarObj == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.FileNotFound,
                    Message = Localizer["FileNotFound"]
                });
            }
            
            Domain.Models.Announcement announcementObj = new Domain.Models.Announcement
            {
                AnnouncementTitle = request.Title,
                AnnouncementDescription = request.Description,
                BaseUser = user,
                UserId = request.UserId,
                Avatar = avatarObj,
                AvatarId = request.Avatar,
                CreatedDate = DateTime.Now
            };

            await _context.Announcements.AddAsync(announcementObj, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new AddAnnouncementViewModel
            {
                Announcement = Mapper.Map<SearchAnnouncementDto>(announcementObj)
            };
        }
    }
}