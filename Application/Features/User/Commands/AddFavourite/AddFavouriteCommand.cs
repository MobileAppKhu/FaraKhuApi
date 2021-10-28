using MediatR;

namespace Application.Features.User.Commands.AddFavourite
{
    public class AddFavouriteCommand : IRequest<AddFavouriteViewModel>
    {
        public string Description { get; set; }
    }
}