using MediatR;

namespace Application.Features.News.Command.AddNews
{
    public class AddNewsCommand : IRequest<AddNewsViewModel>
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}