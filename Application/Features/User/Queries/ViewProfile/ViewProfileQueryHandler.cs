﻿using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.User;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Queries.ViewProfile
{
    public class ViewProfileQueryHandler : IRequestHandler<ViewProfileQuery, ViewProfileViewModel>
    {
        private readonly IMapper _mapper;

        public UserManager<BaseUser> UserManager { get; }

        public IStringLocalizer<SharedResource> Localizer { get; }
        
        private IHttpContextAccessor HttpContextAccessor { get; }

        private readonly IDatabaseContext _context;

        public ViewProfileQueryHandler(IMapper mapper, UserManager<BaseUser> userManager,
            IStringLocalizer<SharedResource> localizer, IHttpContextAccessor httpContextAccessor
            , IDatabaseContext context)
        {
            _mapper = mapper;
            UserManager = userManager;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public async Task<ViewProfileViewModel> Handle(ViewProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.BaseUsers.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            BaseUser baseUser = null;
            switch (user.UserType)
            {
                case UserType.Instructor:
                    baseUser = _context.Instructors.FirstOrDefault(i => i.Id == request.UserId);
                    break;
                case UserType.Student:
                    baseUser = _context.Students.FirstOrDefault(s => s.Id == request.UserId);
                    break;
            }

            return new ViewProfileViewModel
            {
                Profile = _mapper.Map<ProfileDto>(baseUser)
            };

        }
    }
}