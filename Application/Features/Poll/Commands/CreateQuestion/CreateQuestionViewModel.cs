using Application.DTOs.Offer;
using Application.DTOs.Poll;

namespace Application.Features.Poll.Commands.CreateQuestion
{
    public class CreateQuestionViewModel
    {
        public PollQuestionDto Poll { get; set; }
    }
}