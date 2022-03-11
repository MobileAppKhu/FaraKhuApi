using Domain.Enum;
using MediatR;

namespace Application.Features.News.Commands.AddComment

{
    public class AddCommentCommand : IRequest<Unit>
    {
        public string Text { get; set; }
        public string ParentId { get; set; }
        public string NewsId { get; set; }
    }
}