using System.Threading.Tasks;
using Application.Features.Ticket.Commands.AddTicket;
using Application.Features.Ticket.Commands.DeleteTicket;
using Application.Features.Ticket.Commands.EditTicket;
using Application.Features.Ticket.Queries.SearchTicket;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Utilities;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TicketController : ControllerBase
{
    private readonly IMediator _mediator;
    public TicketController(IMediator mediator)
    {
        _mediator = mediator;
    }
        
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AddTicketCommandViewModel), 200)]
    public async Task<IActionResult> AddTicket(AddTicketCommand request)
    {
        request.UserId = this.GetUserId();
        return Ok(await _mediator.Send(request));
    }
        
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> EditTicket(EditTicketCommand request)
    {
        request.UserId = this.GetUserId();
        return Ok(await _mediator.Send(request));
    }
        
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> DeleteTicket(DeleteTicketCommand request)
    {
        request.UserId = this.GetUserId();
        return Ok(await _mediator.Send(request));
    }
        
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SearchTicketQueryViewModel), 200)]
    public async Task<IActionResult> SearchTicket(SearchTicketQuery request)
    {
        return Ok(await _mediator.Send(request));
    }
}