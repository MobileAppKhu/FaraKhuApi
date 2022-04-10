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

        public DeleteAnnouncementCommandHandler(IStringLocalizer<SharedResource> localizer
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
        }
        public async Task<Unit> Handle(DeleteAnnouncementCommand request, CancellationToken cancellationToken)
        {
            var user = _context.BaseUsers.FirstOrDefault(u => u.Id == request.UserId);
            var announcementObj = _context.Announcements.Include(announce => announce.BaseUser)
                .FirstOrDefault(announce => announce.AnnouncementId == request.AnnouncementId);
            if (announcementObj == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.AnnouncementNotFound,
                    Message = Localizer["AnnouncementNotFound"]
                });
            }

            if (announcementObj.BaseUser != user && user.UserType != UserType.Owner)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                }); 
            }
            
            _context.Announcements.Remove(announcementObj);
            
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}