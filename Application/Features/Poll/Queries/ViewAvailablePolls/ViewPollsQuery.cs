using MediatR;

namespace Application.Features.Poll.Queries.ViewAvailablePolls
{
    public class ViewPollsQuery : IRequest<ViewPollsViewModel>
    {
        public int CourseId { get; set; }
    }
}