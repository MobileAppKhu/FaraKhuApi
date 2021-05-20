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
using Application.Features.Announcement.Queries.ViewInstructorAnnouncements;

namespace WebApi.Controllers
{

    [ApiController]
    [Authorize(Policy = "InstructorPolicy")]
    [Route("api/[controller]/[action]")]
    public class AnnouncementController : ControllerBase
    {
        private IMediator _mediator;
        public AnnouncementController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [ProducesResponseType(typeof(CreateAnnouncementViewModel), 200)]
        public async Task<IActionResult> CreateAnnouncement(CreateAnnouncementCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [ProducesResponseType(typeof(RemoveAnnouncementCommand), 200)]
        public async Task<IActionResult> RemoveAnnouncement(RemoveAnnouncementCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ViewAnnouncementsViewModel), 200)]
        public async Task<IActionResult> ViewAnnouncements(ViewAnnouncementsQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ViewInstructorAnnouncementsViewModel), 200)]
        public async Task<IActionResult> ViewInstructorAnnouncements(ViewInstructorAnnouncementsQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        
    }
}