using MediatR;

namespace Application.Features.Poll.Commands.DeleteQuestion
{
    public class RemoveQuestionCommand : IRequest<Unit>
    {
        public string QuestionId { get; set; }
    }
}