using MediatR;

namespace Application.Features.News.Queries.SearchIndividualNews
{
    public class SearchIndividualNewsQuery : IRequest<SearchIndividualNewsViewModel>
    {
        public string NewsId { get; set; }
    }
}