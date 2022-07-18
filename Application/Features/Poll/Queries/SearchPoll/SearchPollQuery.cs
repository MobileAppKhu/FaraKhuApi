using MediatR;

namespace Application.Features.Poll.Queries.SearchPoll;

public class SearchPollQuery : IRequest<SearchPollViewModel>
{
    public string QuestionId { get; set; }
}