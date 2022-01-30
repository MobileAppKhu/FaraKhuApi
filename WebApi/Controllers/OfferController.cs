
using System.Threading.Tasks;
using Application.Features.Offer.Commands.AddOffer;
using Application.Features.Offer.Commands.DeleteOffer;
using Application.Features.Offer.Commands.EditOffer;
using Application.Features.Offer.Queries.SearchOffers;
using Application.Features.Offer.Queries.SearchUserOffers;
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
        [ProducesResponseType(typeof(AddOfferViewModel), 200)]
        public async Task<IActionResult> AddOffer(AddOfferCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteOffer(DeleteOfferCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        public async Task<IActionResult> EditOffer(EditOfferCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [ProducesResponseType(typeof(SearchUserOffersViewModel), 200)]
        public async Task<IActionResult> SearchUserOffers(SearchUserOffersQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(SearchOffersViewModel), 200)]
        public async Task<IActionResult> SearchOffers(SearchOffersQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}