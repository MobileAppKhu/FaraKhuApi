using Application.Common.Mappings;
using AutoMapper;
using Domain.Models;

namespace Application.DTOs.Poll
{
    public class PollQuestionShortDto : IMapFrom<PollQuestion>
    {
        public string QuestionId { get; set; }
        public string QuestionDescription { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PollQuestion, PollQuestionShortDto>();
        }
    }
}