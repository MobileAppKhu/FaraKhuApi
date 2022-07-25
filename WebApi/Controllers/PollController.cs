using System.Threading.Tasks;
using Application.Features.Poll.Commands.AddQuestion;
using Application.Features.Poll.Commands.DeleteQuestion;
using Application.Features.Poll.Commands.RetractVote;
using Application.Features.Poll.Commands.EditQuestion;
using Application.Features.Poll.Commands.Vote;
using Application.Features.Poll.Queries.SearchAvailablePolls;
using Application.Features.Poll.Queries.SearchPoll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Utilities;

namespace WebApi.Controllers;

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
    public async Task<IActionResult> AddPollQuestion(AddQuestionCommand request)
    {
        request.UserId = this.GetUserId();
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    [Authorize(Policy = "InstructorPolicy")]
    public async Task<IActionResult> DeletePollQuestion(DeleteQuestionCommand request)
    {
        request.UserId = this.GetUserId();
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    [Authorize(Policy = "InstructorPolicy")]
    public async Task<IActionResult> EditPollQuestion(EditQuestionCommand request)
    {
        request.UserId = this.GetUserId();
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    [Authorize(Policy = "StudentPolicy")]
    [ProducesResponseType(typeof(RetractVoteViewModel), 200)]
    public async Task<IActionResult> RetractVote(RetractVoteCommand request)
    {
        request.UserId = this.GetUserId();
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    [Authorize(Policy = "StudentPolicy")]
    [ProducesResponseType(typeof(VoteViewModel), 200)]
    public async Task<IActionResult> Vote(VoteCommand request)
    {
        request.UserId = this.GetUserId();
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