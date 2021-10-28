using Application.Features.Announcement.Commands.AddAnnouncement;
using Application.Features.Announcement.Commands.RemoveAnnouncement;
using Application.Features.Announcement.Queries.SearchAnnouncements;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Announcement.Queries.SearchInstructorAnnouncements;

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
        [ProducesResponseType(typeof(AddAnnouncementViewModel), 200)]
        public async Task<IActionResult> AddAnnouncement(AddAnnouncementCommand request)
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
        [ProducesResponseType(typeof(SearchAnnouncementsViewModel), 200)]
        public async Task<IActionResult> SearchAnnouncements(SearchAnnouncementsQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [ProducesResponseType(typeof(SearchInstructorAnnouncementsViewModel), 200)]
        public async Task<IActionResult> SearchInstructorAnnouncements(SearchInstructorAnnouncementsQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        
    }
}