using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Favourite;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Commands.AddFavourite
{
    public class AddFavouriteCommandHandler : IRequestHandler<AddFavouriteCommand, AddFavouriteViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private UserManager<BaseUser> UserManager { get; }
        private IMapper _mapper { get; }

        public AddFavouriteCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            UserManager = userManager;
            _mapper = mapper;
        }
        public async Task<AddFavouriteViewModel> Handle(AddFavouriteCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.BaseUsers.Include(u => u.Favourites).
                FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if(user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            var favourite = new Favourite
            {
                Description = request.Description,
                BaseUser = user,
                UserId = userId
            };

            await _context.Favourites.AddAsync(favourite, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new AddFavouriteViewModel
            {
                Favourite = _mapper.Map<FavouriteDto>(favourite)
            };
        }
    }
}