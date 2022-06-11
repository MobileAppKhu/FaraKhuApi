using System.Text.Json.Serialization;
using Domain.Enum;
using MediatR;

namespace Application.Features.Poll.Commands.RetractVote
{
    public class RetractVoteCommand : IRequest<RetractVoteViewModel>
    {
        [JsonIgnore] public string UserId { get; set; }
        public string AnswerId { get; set; }
    }
}