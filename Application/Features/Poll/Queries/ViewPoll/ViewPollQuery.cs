using MediatR;

namespace Application.Features.Poll.Queries.ViewPoll
{
    public class ViewPollQuery : IRequest<ViewPollViewModel>
    {
        public string QuestionId { get; set; }
    }
}