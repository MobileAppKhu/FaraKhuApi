using MediatR;

namespace Application.Features.News.Command.UpdateNews
{
    public class UpdateNewsCommand : IRequest<Unit>
    {
        public string NewsId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileId { get; set; }
    }
}