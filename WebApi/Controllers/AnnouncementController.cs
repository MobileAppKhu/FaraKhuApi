using Application.Features.Announcement.Commands.AddAnnouncement;
using Application.Features.Announcement.Queries.SearchAnnouncements;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Features.Announcement.Commands.DeleteAnnouncement;
using Application.Features.Announcement.Commands.EditAnnouncement;

namespace WebApi.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IMediator _mediator;
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
        public async Task<IActionResult> DeleteAnnouncement(DeleteAnnouncementCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditAnnouncement(EditAnnouncementCommand request)
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
    }
}