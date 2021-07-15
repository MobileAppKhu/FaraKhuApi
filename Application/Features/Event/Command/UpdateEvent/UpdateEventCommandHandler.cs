﻿using System;
using System.Linq;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Event.Command.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }

        public UpdateEventCommandHandler(IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            BaseUser user = await _context.BaseUsers.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if(user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            var eventObj = _context.Events.FirstOrDefault(e => e.EventId == request.EventId);

            if (eventObj != null)
            {
                if (!string.IsNullOrWhiteSpace(request.EventName))
                {
                    eventObj.EventName = request.EventName;
                }
                if (!string.IsNullOrWhiteSpace(request.EventDescription))
                {
                    eventObj.EventDescription = request.EventDescription;
                }
                if (!string.IsNullOrWhiteSpace(request.EventTime))
                {
                    eventObj.EventTime = DateTimeOffset.Parse(request.EventTime).Date;
                }

                _context.Events.Update(eventObj);
                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.EventNotFound,
                    Message = Localizer["EventNotFound"]
                });
            }

            
            return Unit.Value;
        }
    }
}