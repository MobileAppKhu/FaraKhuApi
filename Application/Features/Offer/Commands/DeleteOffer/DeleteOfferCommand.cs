using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Offer.Commands.DeleteOffer;

public class DeleteOfferCommand : IRequest<Unit>
{
    [JsonIgnore] public string UserId { get; set; }
    public string OfferId { get; set; }
}