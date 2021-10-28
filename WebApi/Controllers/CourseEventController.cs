using System.Threading.Tasks;
using Application.Features.Course.Commands.EditCourse;
using Application.Features.CourseEvent.Commands.AddCourseEvent;
using Application.Features.CourseEvent.Commands.DeleteCourseEvent;
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
        public async Task<IActionResult> DeleteCourseEvent(DeleteCourseEventCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}