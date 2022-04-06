using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Announcement.Commands.DeleteAnnouncement
{
    public class DeleteAnnouncementCommandHandler : IRequestHandler<DeleteAnnouncementCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IMapper Mapper { get; }

        public DeleteAnnouncementCommandHandler(IStringLocalizer<SharedResource> localizer, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            Mapper = mapper;
        }
        public async Task<Unit> Handle(DeleteAnnouncementCommand request, CancellationToken cancellationToken)
        {
            BaseUser user = _context.BaseUsers.FirstOrDefault(u => u.Id == request.UserId);
            var announcementObj = _context.Announcements.Include(announce => announce.BaseUser)
                .FirstOrDefault(announce => announce.AnnouncementId == request.AnnouncementId);
            if (user == null || 
                announcementObj?.BaseUser.Id != user.Id)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            if (announcementObj != null)
                _context.Announcements.Remove(announcementObj);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}