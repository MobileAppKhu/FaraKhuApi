using System.Threading.Tasks;
using Application.Features.Course.Command.UpdateCourse;
using Application.Features.CourseEvent.Command.AddCourseEvent;
using Application.Features.CourseEvent.Command.RemoveCourseEvent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize(Policy = "InstructorPolicy")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CourseEventController : ControllerBase
    {
        private IMediator _mediator;
        public CourseEventController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(AddCourseEventViewModel), 200)]
        public async Task<IActionResult> AddCourseEvent(AddCourseEventCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveCourseEvent(RemoveCourseEventCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}