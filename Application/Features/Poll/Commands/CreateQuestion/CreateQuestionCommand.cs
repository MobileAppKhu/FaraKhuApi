using Domain.Enum;
using MediatR;

namespace Application.Features.Poll.Commands.CreateQuestion
{
    public class CreateQuestionCommand : IRequest<CreateQuestionViewModel>
    {
        public string QuestionDescription { get; set; }
        public bool MultiVote { get; set; } // Check if poll allows MultiVote
        public int CourseId { get; set; }
    }
}