using Domain.Enum;
using MediatR;

namespace Application.Features.Offer.Command.UpdateCommand
{
    public class UpdateOfferCommand : IRequest<Unit>
    {
        public string OfferId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public OfferType OfferType { get; set; }
        public string Price { get; set; }
        public string AvatarId { get; set; }
    }
}