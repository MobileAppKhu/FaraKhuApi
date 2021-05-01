using Application.Common.Mappings;
using AutoMapper;


namespace Application.DTOs.Offer
{
    public class UserOfferDto : IMapFrom<Domain.Models.Offer>
    {
        public string UserFullName { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string OfferType { get; set; }
        
        public string Price { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Offer, UserOfferDto>()
                .ForMember(o => o.UserFullName,
                    opt =>
                        opt.MapFrom(src => src.BaseUser.FirstName + " "
                                                                  + src.BaseUser.LastName))
                .ForMember(o => o.OfferType,
                    opt =>
                        opt.MapFrom(src => src.OfferType.ToString()))
                .ForMember(o => o.OfferType,
                    opt =>
                        opt.MapFrom(src => src.OfferType.ToString()));
        }
    }
}