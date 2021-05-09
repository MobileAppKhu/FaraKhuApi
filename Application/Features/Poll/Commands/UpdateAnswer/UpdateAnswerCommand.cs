using MediatR;

namespace Application.Features.Poll.Commands.UpdateAnswer
{
    public class UpdateAnswerCommand : IRequest<Unit>
    {
        public string AnswerId { get; set; }
        public string AnswerDescription { get; set; }
    }
}