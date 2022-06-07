using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.News.Commands.AddComment

{
    public class AddCommentCommand : IRequest
    {
        [JsonIgnore] public string UserId { get; set; }
        public string Text { get; set; }
        public string ParentId { get; set; }
        public string NewsId { get; set; }
    }
}