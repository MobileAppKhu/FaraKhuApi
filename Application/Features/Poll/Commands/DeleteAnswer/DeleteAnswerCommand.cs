using Domain.Enum;
using MediatR;

namespace Application.Features.Poll.Commands.DeleteAnswer
{
    public class DeleteAnswerCommand : IRequest<DeleteAnswerViewModel>
    {
        public string AnswerId { get; set; }
    }
}