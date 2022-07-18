using System;
using System.Text.Json.Serialization;
using Domain.Enum;
using MediatR;

namespace Application.Features.Event.Queries.SearchEvent;

public class SearchEventQuery : IRequest<SearchEventViewModel>
{
    [JsonIgnore] public string UserId { get; set; }
    public string EventId { get; set; }
    public string EventName { get; set; }
    public string Description { get; set; }
    public DateTime? EventTime { get; set; }
    public string CourseId { get; set; }
    public int Start { get; set; }
    public int Step { get; set; }
    public EventColumn EventColumn { get; set; }
    public bool OrderDirection { get; set; }
}