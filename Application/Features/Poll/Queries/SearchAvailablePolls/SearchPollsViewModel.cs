using System.Collections.Generic;
using Application.DTOs.Poll;

namespace Application.Features.Poll.Queries.SearchAvailablePolls
{
    public class SearchPollsViewModel
    {
        public ICollection<PollQuestionShortDto> Polls { get; set; }
        public int SearchLength { get; set; }
    }
}