using Application.Features.Time.Command.AddTime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TimeController : ControllerBase
    {
        private IMediator _mediator;
        public TimeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Authorize(Policy = "InstructorPolicy")]
        [ProducesResponseType(typeof(AddTimeViewModel), 200)]
        public async Task<IActionResult> AddTime(AddTimeCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

    }
}
