using MediatR;

namespace Application.Features.User.Commands.UpdateFavourite
{
    public class UpdateFavouriteCommand : IRequest<Unit>
    {
        public string FavouriteId { get; set; }
        public string Description { get; set; }
    }
}