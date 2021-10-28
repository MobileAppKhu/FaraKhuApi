using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Commands.UpdateFavourite
{
    public class UpdateFavouriteCommandHandler : IRequestHandler<UpdateFavouriteCommand>
    {
        
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private UserManager<BaseUser> UserManager { get; }
        private IMapper _mapper { get; }

        public UpdateFavouriteCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            UserManager = userManager;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateFavouriteCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.BaseUsers.FirstOrDefaultAsync(u => u.Id == userId,cancellationToken);
            var favourite = await _context.Favourites.Include(f => f.BaseUser)
                .FirstOrDefaultAsync(f => f.FavouriteId == request.FavouriteId, cancellationToken);
            if(favourite.UserId != userId)
                if(user == null)
                    throw new CustomException(new Error
                    {
                        ErrorType = ErrorType.Unauthorized,
                        Message = Localizer["Unauthorized"]
                    });
            favourite.Description = request.Description;
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}