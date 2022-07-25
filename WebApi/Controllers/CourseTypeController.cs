using System.Threading.Tasks;
using Application.Features.CourseType.Queries.SearchCourseType;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class CourseTypeController : ControllerBase
{
    private IMediator _mediator;
    public CourseTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }
        
    [HttpPost]
    [ProducesResponseType(typeof(SearchCourseTypeViewModel), 200)]
    public async Task<IActionResult> SearchCourseType(SearchCourseTypeQuery request)
    {
        return Ok(await _mediator.Send(request));
    }
}