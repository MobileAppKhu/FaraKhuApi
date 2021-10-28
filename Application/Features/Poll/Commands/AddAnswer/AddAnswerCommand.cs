using Domain.Enum;
using MediatR;

namespace Application.Features.Poll.Commands.AddAnswer
{
    public class AddAnswerCommand : IRequest<AddAnswerViewModel>
    {
        public string AnswerDescription { get; set; }
        public string QuestionId { get; set; }
    }
}