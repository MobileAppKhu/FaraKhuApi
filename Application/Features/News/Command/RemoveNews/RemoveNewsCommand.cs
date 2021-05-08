using MediatR;

namespace Application.Features.News.Command.RemoveNews
{
    public class RemoveNewsCommand : IRequest<Unit>
    {
        public string NewsId { get; set; }
    }
}