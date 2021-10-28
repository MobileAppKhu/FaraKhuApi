using MediatR;

namespace Application.Features.User.Commands.RemoveFavourite
{
    public class RemoveFavouriteCommand : IRequest<Unit>
    {
        public string FavouriteId { get; set; }
    }
}