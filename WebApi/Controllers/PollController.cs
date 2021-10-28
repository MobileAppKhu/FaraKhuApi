using System.Threading.Tasks;
using Application.Features.Poll.Commands.AddAnswer;
using Application.Features.Poll.Commands.AddQuestion;
using Application.Features.Poll.Commands.DeleteAnswer;
using Application.Features.Poll.Commands.RemoveQuestion;
using Application.Features.Poll.Commands.RetractVote;
using Application.Features.Poll.Commands.EditAnswer;
using Application.Features.Poll.Commands.EditQuestion;
using Application.Features.Poll.Commands.Vote;
using Application.Features.Poll.Queries.SearchAvailablePolls;
using Application.Features.Poll.Queries.SearchPoll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PollController : ControllerBase
    {
        private IMediator _mediator;
        public PollController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Authorize(Policy = "InstructorPolicy")]
        [ProducesResponseType(typeof(AddQuestionViewModel), 200)]
        public async Task<IActionResult> CreatePollQuestion(AddQuestionCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [Authorize(Policy = "InstructorPolicy")]
        [ProducesResponseType(typeof(AddAnswerViewModel), 200)]
        public async Task<IActionResult> CreatePollAnswer(AddAnswerCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [Authorize(Policy = "InstructorPolicy")]
        [ProducesResponseType(typeof(DeleteAnswerViewModel), 200)]
        public async Task<IActionResult> RemovePollAnswer(DeleteAnswerCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [Authorize(Policy = "InstructorPolicy")]
        public async Task<IActionResult> RemovePollQuestion(RemoveQuestionCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [Authorize(Policy = "InstructorPolicy")]
        public async Task<IActionResult> UpdatePollQuestion(EditQuestionCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [Authorize(Policy = "InstructorPolicy")]
        public async Task<IActionResult> UpdatePollAnswer(EditAnswerCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [Authorize(Policy = "StudentPolicy")]
        [ProducesResponseType(typeof(RetractVoteViewModel), 200)]
        public async Task<IActionResult> RetractVote(RetractVoteCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [Authorize(Policy = "StudentPolicy")]
        [ProducesResponseType(typeof(VoteViewModel), 200)]
        public async Task<IActionResult> Vote(VoteCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(SearchPollsViewModel), 200)]
        public async Task<IActionResult> SearchAvailablePolls(SearchPollsQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(SearchPollsViewModel), 200)]
        public async Task<IActionResult> SearchPoll(SearchPollQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}