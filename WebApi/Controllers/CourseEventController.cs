using System.Threading.Tasks;
using Application.Features.CourseEvent.Commands.AddCourseEvent;
using Application.Features.CourseEvent.Commands.DeleteCourseEvent;
using Application.Features.CourseEvent.Commands.EditCourseEvent;
using Application.Features.CourseEvent.Queries.SearchCourseEvent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Utilities;

namespace WebApi.Controllers;

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
        request.UserId = this.GetUserId();
        return Ok(await _mediator.Send(request));
    }
        
    [HttpPost]
    public async Task<IActionResult> EditCourseEvent(EditCourseEventCommand request)
    {
        request.UserId = this.GetUserId();
        return Ok(await _mediator.Send(request));
    }
        
    [HttpPost]
    public async Task<IActionResult> DeleteCourseEvent(DeleteCourseEventCommand request)
    {
        request.UserId = this.GetUserId();
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    [ProducesResponseType(typeof(SearchCourseEventQueryHandler), 200)]
    public async Task<IActionResult> SearchCourseEvent(SearchCourseEventQuery request)
    {
        return Ok(await _mediator.Send(request));
    }
}