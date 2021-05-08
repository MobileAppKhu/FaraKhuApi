using Domain.Enum;
using MediatR;

namespace Application.Features.Poll.Commands.RemoveAnswer
{
    public class RemoveAnswerCommand : IRequest<RemoveAnswerViewModel>
    {
        public string AnswerId { get; set; }
    }
}