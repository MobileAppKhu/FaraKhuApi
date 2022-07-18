using System.Threading.Tasks;
using Application.Features.News.Commands.AddComment;
using Application.Features.News.Commands.AddNews;
using Application.Features.News.Commands.CommentApproval;
using Application.Features.News.Commands.DeleteNews;
using Application.Features.News.Commands.EditNews;
using Application.Features.News.Commands.RemoveComment;
using Application.Features.News.Queries.GetComments;
using Application.Features.News.Queries.SearchNews;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Utilities;

namespace WebApi.Controllers;

// [Authorize(Policy = "PROfficerPolicy")]
[ApiController]
[Route("api/[controller]/[action]")]
public class NewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Policy = "PROfficerPolicy")]
    [ProducesResponseType(typeof(AddNewsViewModel), 200)]
    public async Task<IActionResult> AddNews(AddNewsCommand request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    [Authorize(Policy = "PROfficerPolicy")]
    public async Task<IActionResult> DeleteNews(DeleteNewsCommand request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SearchNewsViewModel), 200)]
    public async Task<IActionResult> SearchNews(SearchNewsQuery request)
    {
        return Ok(await _mediator.Send(request));
    }
        
    [HttpPost]
    [Authorize(Policy = "PROfficerPolicy")]
    public async Task<IActionResult> EditNews(EditNewsCommand request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddComment(AddCommentCommand request)
    {
        request.UserId = this.GetUserId();
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> GetComments(CommentsQuery request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    [Authorize(Policy = "PROfficerPolicy")]
    public async Task<IActionResult> CommentApproval(CommentApprovalCommand request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RemoveComment(RemoveCommentCommand request)
    {
        request.UserId = this.GetUserId();
        return Ok(await _mediator.Send(request));
    }
}