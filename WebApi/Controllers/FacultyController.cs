using System.Threading.Tasks;
using Application.Features.Faculty.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class FacultyController : ControllerBase
    {
        private IMediator _mediator;
        public FacultyController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(SearchFacultyViewModel), 200)]
        public async Task<IActionResult> SearchFaculty(SearchFacultyQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}