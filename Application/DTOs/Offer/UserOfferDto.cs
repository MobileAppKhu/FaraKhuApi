using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Enum;


namespace Application.DTOs.Offer
{
    public class UserOfferDto : IMapFrom<Domain.Models.Offer>
    {
        public string OfferId { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public OfferType OfferType { get; set; }
        public string Price { get; set; }
        public string AvatarId { get; set; }
        public DateTime CreatedDate { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Offer, UserOfferDto>()
                .ForMember(o => o.UserFullName,
                    opt =>
                        opt.MapFrom(src => src.BaseUser.FirstName + " "
                                                                  + src.BaseUser.LastName));
        }
    }
}