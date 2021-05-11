using System.Threading.Tasks;
using Application.Features.Poll.Commands.CreateAnswer;
using Application.Features.Poll.Commands.CreateQuestion;
using Application.Features.Poll.Commands.RemoveAnswer;
using Application.Features.Poll.Commands.RetractVote;
using Application.Features.Poll.Commands.Vote;
using Application.Features.Poll.Queries.ViewAvailablePolls;
using Application.Features.Poll.Queries.ViewPoll;
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
        [ProducesResponseType(typeof(CreateQuestionViewModel), 200)]
        public async Task<IActionResult> CreatePollQuestion(CreateQuestionCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [Authorize(Policy = "InstructorPolicy")]
        [ProducesResponseType(typeof(CreateAnswerViewModel), 200)]
        public async Task<IActionResult> CreatePollAnswer(CreateAnswerCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [Authorize(Policy = "InstructorPolicy")]
        [ProducesResponseType(typeof(RemoveAnswerViewModel), 200)]
        public async Task<IActionResult> RemovePollAnswer(RemoveAnswerCommand request)
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
        [ProducesResponseType(typeof(ViewPollsViewModel), 200)]
        public async Task<IActionResult> ViewAvailablePolls(ViewPollsQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ViewPollsViewModel), 200)]
        public async Task<IActionResult> ViewPoll(ViewPollQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}