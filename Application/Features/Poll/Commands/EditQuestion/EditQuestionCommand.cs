using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Poll.Commands.EditQuestion
{
    public class EditQuestionCommand : IRequest<Unit>
    {
        [JsonIgnore] public string UserId { get; set; }
        public string QuestionId { get; set; }
        public string QuestionDescription { get; set; }
        public string MultiVote { get; set; } // Check if poll allows MultiVote
        public string IsOpen { get; set; } // Check if poll is open (or closed)
        public List<string> DeleteAnswers { get; set; }
        public List<string> AddAnswers { get; set; }
    }
}