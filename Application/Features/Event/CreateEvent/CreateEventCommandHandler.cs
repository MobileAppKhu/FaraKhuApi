using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.IServices;
using Application.DTOs;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Application.Features.Event.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, CreateEventViewModel>
    {
        /*private readonly IEventServices _eventServices;
        private readonly IUserServices _userServices;*/
        private readonly IDatabaseContext _context;
        
        private IStringLocalizer<SharedResource> Localizer { get; }
        
        private IHttpContextAccessor HttpContextAccessor { get; }
        
        private UserManager<BaseUser> UserManager { get; }
        private IMapper _mapper { get; }

        public CreateEventCommandHandler(IEventServices eventServices, IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager, IMapper mapper
            , IUserServices userServices, IDatabaseContext context)
        {
            _context = context;
            //_eventServices = eventServices;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            UserManager = userManager;
            _mapper = mapper;
            //_userServices = userServices;
        }

        public async Task<CreateEventViewModel> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var user = _userServices.GetUser(userId);
            var user = (BaseUser)_context.BaseUsers.Where(u => u.Id == userId);
            if(user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            var eventObj = new Domain.Models.Event()
            {
                EventName = request.EventName,
                EventType = request.EventType,
                EventDescription = request.EventDescription,
                EventTime = request.EventTime,
                User =  user,
                UserId = user.Id
            };
            //_eventServices.AddEvent(eventObj);
            await _context.Events.AddAsync(eventObj);
            return new CreateEventViewModel
            {
                EventDto = _mapper.Map<EventShortDto>(eventObj)
            };
        }
    }
}