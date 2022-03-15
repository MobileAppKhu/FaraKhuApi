using MediatR;

namespace Application.Features.Poll.Commands.DeleteQuestion
{
    public class DeleteQuestionCommand : IRequest<Unit>
    {
        public string QuestionId { get; set; }
    }
}