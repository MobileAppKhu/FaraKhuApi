using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.User.Queries.SearchAllEvents;

public class SearchAllEventsQuery : IRequest<SearchAllEventsViewModel>
{
    [JsonIgnore] public string UserId { get; set; }
}