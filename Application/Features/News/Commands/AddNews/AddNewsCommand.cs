using MediatR;

namespace Application.Features.News.Commands.AddNews
{
    public class AddNewsCommand : IRequest<AddNewsViewModel>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileId { get; set; }
    }
}