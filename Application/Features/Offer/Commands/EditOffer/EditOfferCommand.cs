using System.Text.Json.Serialization;
using Domain.Enum;
using MediatR;

namespace Application.Features.Offer.Commands.EditOffer
{
    public class EditOfferCommand : IRequest<Unit>
    {
        [JsonIgnore] public string UserId { get; set; }
        public string OfferId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public OfferType OfferType { get; set; }
        public string Price { get; set; }
        public string AvatarId { get; set; }
    }
}