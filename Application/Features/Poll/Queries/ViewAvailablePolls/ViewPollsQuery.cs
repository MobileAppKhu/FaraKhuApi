using MediatR;

namespace Application.Features.Poll.Queries.ViewAvailablePolls
{
    public class ViewPollsQuery : IRequest<ViewPollsViewModel>
    {
        public string CourseId { get; set; }
        public int Start { get; set; }
        public int Step { get; set; }
    }
}