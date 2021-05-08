using Application.Common.Mappings;
using AutoMapper;
using Domain.Enum;

namespace Application.DTOs.Offer
{
    public class OfferDto : IMapFrom<Domain.Models.Offer>
    {
        public string OfferId { get; set; }
        public string Title { get; set; }
        
        public string Description { get; set; }
        public string OfferType { get; set; }
        
        public string Price { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Offer, OfferDto>()
                .ForMember(o => o.OfferType,
                    opt =>
                        opt.MapFrom(src => src.OfferType.ToString()));
        }
    }
}