using Application.Features.Announcement.Commands.CreateAnnouncement;
using Application.Features.Announcement.Commands.RemoveAnnouncement;
using Application.Features.Announcement.Queries.ViewAnnouncements;
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
    public class AnnouncementController : ControllerBase
    {
        private IMediator _mediator;
        public AnnouncementController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Authorize(Policy = "InstructorPolicy")]
        [ProducesResponseType(typeof(CreateAnnouncementViewModel), 200)]
        public async Task<IActionResult> CreateAnnouncement(CreateAnnouncementCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Authorize(Policy = "InstructorPolicy")]
        [ProducesResponseType(typeof(RemoveAnnouncementCommand), 200)]
        public async Task<IActionResult> RemoveAnnouncement(RemoveAnnouncementCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ViewAnnouncementsViewModel), 200)]
        public async Task<IActionResult> ViewAnnouncements(ViewAnnouncementsQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        
    }
}