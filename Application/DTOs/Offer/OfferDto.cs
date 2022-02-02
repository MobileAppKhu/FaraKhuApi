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
        public OfferType OfferType { get; set; }
        public string Price { get; set; }
        public string AvatarId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Offer, OfferDto>();
        }
    }
}