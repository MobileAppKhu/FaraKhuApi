using MediatR;

namespace Application.Features.User.Command.RemoveFavourite
{
    public class RemoveFavouriteCommand : IRequest<Unit>
    {
        public string FavouriteId { get; set; }
    }
}