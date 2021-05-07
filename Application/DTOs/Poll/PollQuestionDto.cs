using System.Collections.Generic;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Models;

namespace Application.DTOs.Poll
{
    public class PollQuestionDto : IMapFrom<PollQuestionDto>
    {
        public int QuestionId { get; set; }
        public string QuestionDescription { get; set; }
        public ICollection<PollAnswerDto> Answers { get; set; }
        public bool MultiVote { get; set; } // Check if poll allows MultiVote
        public bool IsOpen { get; set; } // Check if poll is open (or closed)

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PollQuestion, PollQuestionDto>();
        }
    }
}