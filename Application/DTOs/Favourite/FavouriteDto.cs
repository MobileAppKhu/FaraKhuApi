using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.Favourite
{
    public class FavouriteDto : IMapFrom<Domain.Models.Favourite>
    {
        public string FavouriteId { get; set; }
        public string Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Favourite, FavouriteDto>();
        }
    }
}