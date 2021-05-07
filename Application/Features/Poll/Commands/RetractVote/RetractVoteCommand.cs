using Domain.Enum;
using MediatR;

namespace Application.Features.Poll.Commands.RetractVote
{
    public class RetractVoteCommand : IRequest<RetractVoteViewModel>
    {
        public int AnswerId { get; set; }
    }
}