using MediatR;

namespace Application.Features.News.Commands.RemoveNews
{
    public class RemoveCommentCommand : IRequest<Unit>
    {
        public string CommentId { get; set; }
    }
}