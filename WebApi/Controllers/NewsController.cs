﻿using System.Threading.Tasks;
using Application.Features.News.Commands.AddNews;
using Application.Features.News.Commands.DeleteNews;
using Application.Features.News.Commands.EditNews;
using Application.Features.News.Queries.SearchIndividualNews;
using Application.Features.News.Queries.SearchNews;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize(Policy = "PROfficerPolicy")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NewsController : ControllerBase
    {
        private IMediator _mediator;

        public NewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddNewsViewModel), 200)]
        public async Task<IActionResult> AddNews(AddNewsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNews(DeleteNewsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SearchNewsViewModel), 200)]
        public async Task<IActionResult> SearchNews(SearchNewsQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SearchIndividualNewsViewModel), 200)]
        public async Task<IActionResult> SearchIndividualNews(SearchIndividualNewsQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> EditNews(EditNewsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}