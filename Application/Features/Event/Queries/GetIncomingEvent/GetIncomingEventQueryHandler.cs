using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.User;
using Application.Features.User.Queries.SearchAllEvents;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Event.Queries.GetIncomingEvent
{
    public class GetIncomingEventQueryHandler : IRequestHandler<GetIncomingEventQuery, GetIncomingEventViewModel>
    {
        private readonly IMapper _mapper;
        public UserManager<BaseUser> UserManager { get; }

        public IStringLocalizer<SharedResource> Localizer { get; }
        
        private IHttpContextAccessor HttpContextAccessor { get; }

        private readonly IDatabaseContext _context;

        public GetIncomingEventQueryHandler(IMapper mapper, UserManager<BaseUser> userManager,
            IStringLocalizer<SharedResource> localizer, IHttpContextAccessor httpContextAccessor
            , IDatabaseContext context)
        {
            _mapper = mapper;
            UserManager = userManager;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _context = context;
        }
        
        public async Task<GetIncomingEventViewModel> Handle(GetIncomingEventQuery request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            var serverTime = DateTime.Now;
            BaseUser baseUser = await _context.BaseUsers.Include(bu => bu.Events)
                .FirstOrDefaultAsync(bu => bu.Id == userId, cancellationToken);
            baseUser.Events = baseUser.Events.Where(e => e.isDone == false && e.EventTime.CompareTo(serverTime) > 0)
                .OrderBy(e => e.EventTime).Take(3).ToList();

            return new GetIncomingEventViewModel
            {
                Events = _mapper.Map<IncomingEventDto>(baseUser),
            };
        }
    }
}