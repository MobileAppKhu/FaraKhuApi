using System;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Offer.Commands.AddOffer
{
    public class AddOfferCommandHandler : IRequestHandler<AddOfferCommand, AddOfferViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IMapper _mapper { get; }

        public AddOfferCommandHandler( IStringLocalizer<SharedResource> localizer,
            IMapper mapper, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            _mapper = mapper;
        }
        public async Task<AddOfferViewModel> Handle(AddOfferCommand request, CancellationToken cancellationToken)
        {
            BaseUser user = await _context.BaseUsers.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            var avatarObj = await _context.Files.FirstOrDefaultAsync(a => a.Id == request.AvatarId,
                cancellationToken);
            if (avatarObj == null)
                throw new CustomException(new Error
                    {ErrorType = ErrorType.FileNotFound, Message = Localizer["FileNotFound"]});
            
            Domain.Models.Offer offer = new Domain.Models.Offer
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                OfferType = request.OfferType,
                BaseUser = user,
                UserId = user.Id,
                Avatar = avatarObj,
                AvatarId = request.AvatarId,
                CreatedDate = DateTime.Now
            };
            await _context.Offers.AddAsync(offer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new AddOfferViewModel
            {
                Offer = _mapper.Map<OfferDto>(offer)
            };
        }
    }
}