using System.Collections.Generic;
using System.Text.Json.Serialization;
using Domain.Enum;
using MediatR;

namespace Application.Features.Poll.Commands.AddQuestion
{
    public class AddQuestionCommand : IRequest<AddQuestionViewModel>
    {
        [JsonIgnore] public string UserId { get; set; }
        public string QuestionDescription { get; set; }
        public string MultiVote { get; set; } // Check if poll allows MultiVote
        public string CourseId { get; set; }

        public List<string> Answers { get; set; }
    }
}