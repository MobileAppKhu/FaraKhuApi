using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.News.Commands.RemoveComment
{
    public class RemoveCommentCommand : IRequest<Unit>
    {
        [JsonIgnore] public string UserId { get; set; }
        public string CommentId { get; set; }
    }
}