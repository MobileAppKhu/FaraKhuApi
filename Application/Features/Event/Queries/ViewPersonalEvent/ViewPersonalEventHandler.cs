using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Event.PersonalEvent;
using Application.DTOs.User;
using Application.Features.User.Queries.ViewAllEvents;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Event.Queries.ViewPersonalEvent
{
    public class ViewPersonalEventHandler : IRequestHandler<ViewPersonalEventQuery, ViewPersonalEventViewModel>
    {
        private readonly IMapper _mapper;

        public UserManager<BaseUser> UserManager { get; }

        public IStringLocalizer<SharedResource> Localizer { get; }
        
        private IHttpContextAccessor HttpContextAccessor { get; }

        private readonly IDatabaseContext _context;

        public ViewPersonalEventHandler(IMapper mapper, UserManager<BaseUser> userManager,
            IStringLocalizer<SharedResource> localizer, IHttpContextAccessor httpContextAccessor
            , IDatabaseContext context)
        {
            _mapper = mapper;
            UserManager = userManager;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public async Task<ViewPersonalEventViewModel> Handle(ViewPersonalEventQuery request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            Domain.Models.Event eventObj = _context.Events.FirstOrDefault(e => e.EventId == request.EventId);
            
            return new ViewPersonalEventViewModel
            {
                Event = _mapper.Map<EventDto>(eventObj)
            };
        }
    }
}