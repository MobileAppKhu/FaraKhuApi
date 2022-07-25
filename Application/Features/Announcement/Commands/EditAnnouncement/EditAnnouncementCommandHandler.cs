using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Announcement.Commands.EditAnnouncement;

public class EditAnnouncementCommandHandler : IRequestHandler<EditAnnouncementCommand>
{
    private readonly IDatabaseContext _context;
    private IStringLocalizer<SharedResource> Localizer { get; }

    public EditAnnouncementCommandHandler(IStringLocalizer<SharedResource> localizer
        , IDatabaseContext context)
    {
        _context = context;
        Localizer = localizer;
    }

    public async Task<Unit> Handle(EditAnnouncementCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.BaseUsers.FirstOrDefaultAsync(baseUser => baseUser.Id == request.UserId,
            cancellationToken);

        var editingAnnouncement = await _context.Announcements.FirstOrDefaultAsync(
            announcement => announcement.AnnouncementId == request.AnnouncementId, cancellationToken);
        if (editingAnnouncement == null)
        {
            throw new CustomException(new Error
            {
                ErrorType = ErrorType.AnnouncementNotFound,
                Message = Localizer["AnnouncementNotFound"]
            });
        }

        if (user.UserType != UserType.Owner && editingAnnouncement.UserId != user.Id)
        {
            throw new CustomException(new Error
            {
                ErrorType = ErrorType.Unauthorized,
                Message = Localizer["Unauthorized"]
            });
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            editingAnnouncement.AnnouncementDescription = request.Description;
        }

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            editingAnnouncement.AnnouncementTitle = request.Title;
        }

        if (!string.IsNullOrWhiteSpace(request.AvatarId))
        {
            var avatar = await
                _context.Files.FirstOrDefaultAsync(entity => entity.Id == request.AvatarId, cancellationToken);
            if (avatar == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.FileNotFound,
                    Message = Localizer["FileNotFound"]
                });
            }
            editingAnnouncement.Avatar = avatar;
            editingAnnouncement.AvatarId = request.AvatarId;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}