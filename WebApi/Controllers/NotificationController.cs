using System.Threading.Tasks;
using Application.Features.Notification.Commands.AddCourseNotification;
using Application.Features.Notification.Commands.DeleteNotification;
using Application.Features.Notification.Queries.SearchNotification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NotificationController : ControllerBase
    {
        private IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [Authorize(Policy = "InstructorPolicy")]
        [ProducesResponseType(typeof(AddCourseNotificationViewModel), 200)]
        public async Task<IActionResult> AddCourseNotification(AddCourseNotificationCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(SearchNotificationViewModel), 200)]
        public async Task<IActionResult> SearchNotification(SearchNotificationQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteNotification(DeleteNotificationCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}