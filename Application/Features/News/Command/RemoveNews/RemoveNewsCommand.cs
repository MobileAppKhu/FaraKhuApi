using MediatR;

namespace Application.Features.News.Command.RemoveNews
{
    public class RemoveNewsCommand : IRequest<Unit>
    {
        public int NewsId { get; set; }
    }
}