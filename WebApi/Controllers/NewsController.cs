using System.Threading.Tasks;
using Application.Features.News.Command.AddNews;
using Application.Features.News.Command.RemoveNews;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize(Policy = "PROfficerPolicy")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NewsController : ControllerBase
    {
        private IMediator _mediator;
        public NewsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(AddNewsViewModel), 200)]
        public async Task<IActionResult> AddNews(AddNewsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        public async Task<IActionResult> RemoveNews(RemoveNewsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}