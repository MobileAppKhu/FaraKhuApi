using MediatR;

namespace Application.Features.User.Command.AddFavourite
{
    public class AddFavouriteCommand : IRequest<AddFavouriteViewModel>
    {
        public string Description { get; set; }
    }
}