using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.Commands.EditProfile;

public class EditProfileCommandHandler : IRequestHandler<EditProfileCommand>
{
    private readonly IDatabaseContext _context;
    private IStringLocalizer<SharedResource> Localizer { get; }

    public EditProfileCommandHandler(IStringLocalizer<SharedResource> localizer,
        IDatabaseContext context)
    {
        _context = context;
        Localizer = localizer;
    }

    public async Task<Unit> Handle(EditProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.BaseUsers.FirstOrDefaultAsync(baseUser => baseUser.Id == request.UserId,
            cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.FirstName))
        {
            user.FirstName = request.FirstName;
        }

        if (!string.IsNullOrWhiteSpace(request.LastName))
        {
            user.LastName = request.LastName;
        }

        if (!string.IsNullOrWhiteSpace(request.AvatarId))
        {
            var avatar =
                await _context.Files.FirstOrDefaultAsync(entity => entity.Id == request.AvatarId,
                    cancellationToken);
            user.AvatarId = request.AvatarId;
            user.Avatar = avatar;
        }

        if (request.DeleteAvatar)
        {
            // should use a constant or sth but for now (smiley.png)
            var avatar =
                await _context.Files.FirstOrDefaultAsync(entity => entity.Id == "smiley.png", cancellationToken);
            user.AvatarId = "smiley.png";
            user.Avatar = avatar;
        }

        if (request.AddFavourites.Count != 0)
        {
            foreach (var favouriteDescription in request.AddFavourites)
            {
                await _context.Favourites.AddAsync(new Favourite
                {
                    Description = favouriteDescription,
                    BaseUser = user,
                    UserId = request.UserId
                }, cancellationToken);
            }
        }

        if (request.DeleteFavourites.Count != 0)
        {
            List<Favourite> deletingFavourites = await _context.Favourites.Where(favourite =>
                    request.DeleteFavourites.Contains(favourite.FavouriteId) && favourite.UserId == request.UserId)
                .ToListAsync(cancellationToken);
            if (deletingFavourites.Count != request.DeleteFavourites.Count)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.FavouriteNotFound,
                    Message = Localizer["FavouriteNotFound"]
                });
            }

            foreach (var deletingFavourite in deletingFavourites)
            {
                _context.Favourites.Remove(deletingFavourite);
            }
        }

        if (!string.IsNullOrWhiteSpace(request.LinkedIn))
        {
            user.LinkedIn = request.LinkedIn;
        }

        if (!string.IsNullOrWhiteSpace(request.GoogleScholar))
        {
            user.GoogleScholar = request.GoogleScholar;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}