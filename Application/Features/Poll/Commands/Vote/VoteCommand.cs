using Domain.Enum;
using MediatR;

namespace Application.Features.Poll.Commands.Vote
{
    public class VoteCommand : IRequest<VoteViewModel>
    {
        public string AnswerId { get; set; }
    }
}