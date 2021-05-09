using MediatR;

namespace Application.Features.Poll.Commands.UpdateQuestion
{
    public class UpdateQuestionCommand : IRequest<Unit>
    {
        public string QuestionId { get; set; }
        public string QuestionDescription { get; set; }
        public bool MultiVote { get; set; } // Check if poll allows MultiVote
        public bool IsOpen { get; set; } // Check if poll is open (or closed)
    }
}