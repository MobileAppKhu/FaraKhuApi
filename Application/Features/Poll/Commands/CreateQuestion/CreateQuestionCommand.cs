using Domain.Enum;
using MediatR;

namespace Application.Features.Poll.Commands.CreateQuestion
{
    public class CreateQuestionCommand : IRequest<CreateQuestionViewModel>
    {
        public string QuestionDescription { get; set; }
        public string MultiVote { get; set; } // Check if poll allows MultiVote
        public string CourseId { get; set; }
    }
}