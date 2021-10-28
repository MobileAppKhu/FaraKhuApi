using MediatR;

namespace Application.Features.News.Commands.DeleteNews
{
    public class DeleteNewsCommand : IRequest<Unit>
    {
        public string NewsId { get; set; }
    }
}