using System.Threading.Tasks;
using Application.Features.Course.Command.AddStudent;
using Application.Features.Course.Command.CreateCourse;
using Application.Features.Course.Command.RemoveStudent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CourseController : ControllerBase
    {
        private IMediator _mediator;
        public CourseController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        //[Authorize(Policy = "InstructorPolicy")]
        [ProducesResponseType(typeof(CreateCourseViewModel),200)]
        public async Task<IActionResult> CreateCourse(CreateCourseCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        //[Authorize(Policy = "InstructorPolicy")]
        [ProducesResponseType(typeof(AddStudentViewModel),200)]
        public async Task<IActionResult> AddStudent(AddStudentCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [ProducesResponseType(typeof(RemoveStudentViewModel), 200)]
        public async Task<IActionResult> RemoveStudent(RemoveStudentCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}