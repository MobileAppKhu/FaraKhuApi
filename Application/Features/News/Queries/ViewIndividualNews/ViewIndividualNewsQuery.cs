using MediatR;

namespace Application.Features.News.Queries.ViewIndividualNews
{
    public class ViewIndividualNewsQuery : IRequest<ViewIndividualNewsViewModel>
    {
        public string NewsId { get; set; }
    }
}