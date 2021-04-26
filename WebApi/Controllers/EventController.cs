﻿using System.Threading.Tasks;
using Application.Features.Event.Command.CreateEvent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EventController : ControllerBase
    {
        private IMediator _mediator;
        public EventController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [ProducesResponseType(typeof(CreateEventViewModel),200)]
        public async Task<IActionResult> CreateEvent(CreateEventCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
    }
    
}