using MediatR;

namespace Application.Features.Poll.Commands.RemoveQuestion
{
    public class RemoveQuestionCommand : IRequest<Unit>
    {
        public string QuestionId { get; set; }
    }
}