using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Poll.Commands.Vote;

public class VoteCommand : IRequest<VoteViewModel>
{
    [JsonIgnore] public string UserId { get; set; }
    public string AnswerId { get; set; }
}