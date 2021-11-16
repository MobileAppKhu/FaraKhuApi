using System.Threading.Tasks;
using Application.Features.Course.Commands.AddCourse;
using Application.Features.Course.Commands.DeleteCourse;
using Application.Features.Course.Commands.EditCourse;
using Application.Features.Course.Queries.SearchCourse;
using Application.Features.Course.Queries.SearchMyCourses;
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
        [Authorize(Policy = "InstructorPolicy")]
        [ProducesResponseType(typeof(AddCourseViewModel),200)]
        public async Task<IActionResult> AddCourse(AddCourseCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        [Authorize(Policy = "InstructorPolicy")]
        public async Task<IActionResult> EditCourse(EditCourseCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        [Authorize(Policy = "InstructorPolicy")]
        public async Task<IActionResult> DeleteCourse(DeleteCourseCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(SearchCourseViewModel), 200)]
        public async Task<IActionResult> SearchCourse(SearchCourseQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(SearchMyCoursesViewModel), 200)]
        public async Task<IActionResult> SearchMyCourses(SearchMyCoursesQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}