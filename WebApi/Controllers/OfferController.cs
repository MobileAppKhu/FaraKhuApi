
using System.Threading.Tasks;
using Application.Features.Offer.Command.CreateOffer;
using Application.Features.Offer.Command.RemoveOffer;
using Application.Features.Offer.Command.UpdateCommand;
using Application.Features.Offer.Query.ViewOffers;
using Application.Features.Offer.Query.ViewUserOffers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OfferController : ControllerBase
    {
        private IMediator _mediator;
        public OfferController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(CreateOfferViewModel), 200)]
        public async Task<IActionResult> CreateOffer(CreateOfferCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        public async Task<IActionResult> RemoveOffer(RemoveOfferCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOffer(UpdateOfferCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [ProducesResponseType(typeof(ViewUserOffersViewModel), 200)]
        public async Task<IActionResult> ViewUserOffers(ViewUserOffersQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(ViewOffersViewModel), 200)]
        public async Task<IActionResult> ViewOffers(ViewOffersQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}