using MediatR;

namespace Application.Features.Poll.Queries.SearchAvailablePolls
{
    public class SearchPollsQuery : IRequest<SearchPollsViewModel>
    {
        public string CourseId { get; set; }
        public int Start { get; set; }
        public int Step { get; set; }
    }
}