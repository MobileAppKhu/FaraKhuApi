using Domain.BaseModels;
using Domain.Enum;

namespace Domain.Models
{
    public class Offer : BaseEntity
    {
        public string OfferId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public OfferType OfferType { get; set; }
        public string Price { get; set; }
        public BaseUser BaseUser { get; set; }
        public string UserId { get; set; }
    }
}