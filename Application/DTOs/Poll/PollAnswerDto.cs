using System.Collections.Generic;
using Application.Common.Mappings;
using Application.DTOs.Student;
using AutoMapper;
using Domain.Models;

namespace Application.DTOs.Poll
{
    public class PollAnswerDto : IMapFrom<PollAnswer>
    {
        public string AnswerId { get; set; }
        public string AnswerDescription { get; set; }
        public ICollection<StudentShortDto> Voters { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PollAnswer, PollAnswerDto>();
        }
    }
}