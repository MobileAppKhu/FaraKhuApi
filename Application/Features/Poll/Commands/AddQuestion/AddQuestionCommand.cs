using Domain.Enum;
using MediatR;

namespace Application.Features.Poll.Commands.AddQuestion
{
    public class AddQuestionCommand : IRequest<AddQuestionViewModel>
    {
        public string QuestionDescription { get; set; }
        public string MultiVote { get; set; } // Check if poll allows MultiVote
        public string CourseId { get; set; }
    }
}