using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Ticket.Commands.DeleteTicket;

public class DeleteTicketCommand : IRequest<Unit>
{
    [JsonIgnore] public string UserId { get; set; }
    public string TicketId { get; set; }
}