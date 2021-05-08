using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.News
{
    public class NewsDto : IMapFrom<Domain.Models.News>
    {
        public string NewsId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.News, NewsDto>();
        }
    }
}