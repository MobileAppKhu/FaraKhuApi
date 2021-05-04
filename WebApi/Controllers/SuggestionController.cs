using System.Threading.Tasks;
using Application.Features.Suggestion.Command.CreateSuggestion;
using Application.Features.Suggestion.Command.RemoveSuggestion;
using Application.Features.Suggestion.Queries.ViewSuggestions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SuggestionController : ControllerBase
    {
        private IMediator _mediator;
        public SuggestionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(CreateSuggestionViewModel), 200)]
        public async Task<IActionResult> CreateSuggestion(CreateSuggestionCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [Authorize(Policy = "PROfficerPolicy")]
        public async Task<IActionResult> RemoveSuggestion(RemoveSuggestionCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [Authorize(Policy = "PROfficerPolicy")]
        [ProducesResponseType(typeof(ViewSuggestionsViewModel), 200)]
        public async Task<IActionResult> ViewSuggestion(ViewSuggestionsQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}