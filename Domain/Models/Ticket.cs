using System;
using Domain.BaseModels;
using Domain.Enum;

namespace Domain.Models;

public class Ticket : BaseEntity
{
    public string TicketId { get; set; }
    public string Description { get; set; }
    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }
    public DateTime? DeadLine { get; set; }
    public string CreatorId { get; set; }
    public BaseUser Creator { get; set; }
}