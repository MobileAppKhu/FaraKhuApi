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
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Announcement.Commands.AddAnnouncement
{
    public class AddAnnouncementCommandHandler : IRequestHandler<AddAnnouncementCommand, AddAnnouncementViewModel>
    {
        private readonly IDatabaseContext _context;

        private IStringLocalizer<SharedResource> Localizer { get; }

        private IHttpContextAccessor HttpContextAccessor { get; }


        private IMapper Mapper { get; }

        public AddAnnouncementCommandHandler(IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            Mapper = mapper;
        }

        public async Task<AddAnnouncementViewModel> Handle(AddAnnouncementCommand request,
            CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            BaseUser user = _context.BaseUsers.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            
            // default value computer department because foreign key is not nullable and
            // front does not support foreign key should be removed in feature
            var departmentObj =
                    await _context.Departments.Include(department => department.Faculty)
                        .FirstOrDefaultAsync(department => department.DepartmentTitle == "کامپیوتر",
                            cancellationToken);
                if (departmentObj == null)
                {
                    throw new CustomException(new Error
                    {
                        ErrorType = ErrorType.DepartmentNotFound,
                        Message = Localizer["DepartmentNotFound"]
                    });
                }
            

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
                Department = departmentObj,
                DepartmentId = request.Department,
                BaseUser = user,
                UserId = userId,
                Avatar = avatarObj,
                AvatarId = request.Avatar
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