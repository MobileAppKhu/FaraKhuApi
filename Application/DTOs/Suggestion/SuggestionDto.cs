using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.Suggestion
{
    public class SuggestionDto : IMapFrom<Domain.Models.Suggestion>
    {
        public string SenderFullname { get; set; }
        public string Detail { get; set; }
        public string SuggestionId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Suggestion, SuggestionDto>()
                .ForMember(s => s.SenderFullname,
                    opt =>
                        opt.MapFrom(src => src.Sender.FirstName 
                                           + " " + src.Sender.LastName));
        }
    }
}