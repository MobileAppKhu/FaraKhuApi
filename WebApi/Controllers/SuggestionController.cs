using System.Threading.Tasks;
using Application.Features.Suggestion.Commands.AddSuggestion;
using Application.Features.Suggestion.Commands.RemoveSuggestion;
using Application.Features.Suggestion.Queries.SearchSuggestions;
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
        [ProducesResponseType(typeof(AddSuggestionViewModel), 200)]
        public async Task<IActionResult> AddSuggestion(AddSuggestionCommand request)
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
        [ProducesResponseType(typeof(SearchSuggestionsViewModel), 200)]
        public async Task<IActionResult> SearchSuggestion(SearchSuggestionsQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}