using System.Collections.Generic;
using Application.DTOs.Poll;

namespace Application.Features.Poll.Queries.ViewAvailablePolls
{
    public class ViewPollsViewModel
    {
        public ICollection<PollQuestionDto> Polls { get; set; }
    }
}