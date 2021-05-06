using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Offer;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Application.Features.Offer.Command.CreateOffer
{
    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, CreateOfferViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IMapper _mapper { get; }

        public CreateOfferCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IMapper mapper, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<CreateOfferViewModel> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            BaseUser user = _context.BaseUsers.FirstOrDefault(u => u.Id == userId);
            if(user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            Domain.Models.Offer offer = new Domain.Models.Offer
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                OfferType = request.OfferType,
                BaseUser = user,
                UserId = user.Id
            };
            await _context.Offers.AddAsync(offer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateOfferViewModel
            {
                Offer = _mapper.Map<OfferDto>(offer)
            };
        }
    }
}