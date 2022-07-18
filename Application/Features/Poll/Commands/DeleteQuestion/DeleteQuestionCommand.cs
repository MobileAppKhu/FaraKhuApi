using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Poll.Commands.DeleteQuestion;

public class DeleteQuestionCommand : IRequest<Unit>
{
    [JsonIgnore] public string UserId { get; set; }
    public string QuestionId { get; set; }
}