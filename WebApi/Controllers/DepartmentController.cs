using System.Threading.Tasks;
using Application.Features.Department.Queries.SearchDepartment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private IMediator _mediator;
        public DepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(SearchDepartmentViewModel), 200)]
        public async Task<IActionResult> SearchDepartment(SearchDepartmentQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}