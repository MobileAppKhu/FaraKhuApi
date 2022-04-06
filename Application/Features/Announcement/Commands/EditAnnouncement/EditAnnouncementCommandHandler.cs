using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Announcement.Commands.EditAnnouncement
{
    public class EditAnnouncementCommandHandler : IRequestHandler<EditAnnouncementCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IMapper Mapper { get; }

        public EditAnnouncementCommandHandler(IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            Mapper = mapper;
        }

        public async Task<Unit> Handle(EditAnnouncementCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.BaseUsers.FirstOrDefaultAsync(baseUser => baseUser.Id == request.UserId,
                cancellationToken);
            if (user == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

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

            if (user.UserType != UserType.Owner || editingAnnouncement.UserId != user.Id)
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

            if (!string.IsNullOrWhiteSpace(request.Department))
            {
                var departmentObj =
                    await _context.Departments.FirstOrDefaultAsync(
                        department => department.DepartmentId == request.Department, cancellationToken);
                if (departmentObj == null)
                {
                    throw new CustomException(new Error
                    {
                        ErrorType = ErrorType.DepartmentNotFound,
                        Message = Localizer["DepartmentNotFound"]
                    });
                }

                editingAnnouncement.DepartmentId = request.Department;
                editingAnnouncement.Department = departmentObj;
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
}