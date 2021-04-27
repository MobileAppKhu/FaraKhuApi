﻿using System.Collections.Generic;
using System.Linq;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Queries.ViewAllEvents
{
    public class ViewAllEventsHandler : IRequestHandler<ViewAllEventsQuery, ViewAllEventsViewModel>
    {
        private readonly IMapper _mapper;

        public UserManager<BaseUser> UserManager { get; }

        public IStringLocalizer<SharedResource> Localizer { get; }
        
        private IHttpContextAccessor HttpContextAccessor { get; }

        private readonly IDatabaseContext _context;

        public ViewAllEventsHandler(IMapper mapper, UserManager<BaseUser> userManager,
            IStringLocalizer<SharedResource> localizer, IHttpContextAccessor httpContextAccessor
            , IDatabaseContext context)
        {
            _mapper = mapper;
            UserManager = userManager;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _context = context;
        }
        
        public async Task<ViewAllEventsViewModel> Handle(ViewAllEventsQuery request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            ICollection<string> roles = await UserManager.GetRolesAsync(user);
            BaseUser baseUser;
            if (roles.First() == UserType.Student.ToString())
                baseUser = _context.Students.Include(s => s.Courses).
                    Include(s => s.Events).
                    FirstOrDefault(s => s.Id == userId);
            else
                baseUser = _context.Instructors.Include(s => s.Courses).
                    Include(s => s.Events).
                    FirstOrDefault(s => s.Id == userId);

            return new ViewAllEventsViewModel
            {
                Events = _mapper.Map<AllEventsDto>(baseUser)
            };
        }
    }
}