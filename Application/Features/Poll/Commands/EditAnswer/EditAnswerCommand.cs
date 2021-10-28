using MediatR;

namespace Application.Features.Poll.Commands.EditAnswer
{
    public class EditAnswerCommand : IRequest<Unit>
    {
        public string AnswerId { get; set; }
        public string AnswerDescription { get; set; }
    }
}