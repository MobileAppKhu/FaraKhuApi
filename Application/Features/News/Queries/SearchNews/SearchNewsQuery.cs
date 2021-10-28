using MediatR;

namespace Application.Features.News.Queries.SearchNews
{
    public class SearchNewsQuery : IRequest<SearchNewsViewModel>
    {
        public string Search { get; set; }
        public int Start { get; set; }
        public int Step { get; set; }
    }
}