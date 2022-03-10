using System.Linq;
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

namespace Application.Features.Offer.Commands.DeleteOffer
{
    public class DeleteOfferCommandHandler : IRequestHandler<DeleteOfferCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }

        public DeleteOfferCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            BaseUser user = _context.BaseUsers.Include(u => u.Offers).FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                }); 
            }
                
            Domain.Models.Offer offer = await _context.Offers
                .Include(o => o.Avatar)
                .Include(o => o.BaseUser)
                .FirstOrDefaultAsync(o => o.OfferId == request.OfferId, cancellationToken);
            if (offer == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.OfferNotFound,
                    Message = Localizer["OfferNotFound"]
                });
            }
            
            if (offer.BaseUser != user && user.UserType != UserType.Owner)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                }); 
            }
            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}