using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.News;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Queries.SearchIndividualNews
{
    public class SearchIndividualNewsQueryHandler : IRequestHandler<SearchIndividualNewsQuery, SearchIndividualNewsViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IMapper _mapper { get; }

        public SearchIndividualNewsQueryHandler( IStringLocalizer<SharedResource> localizer
            , IMapper mapper, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            _mapper = mapper;
        }
        public async Task<SearchIndividualNewsViewModel> Handle(SearchIndividualNewsQuery request, CancellationToken cancellationToken)
        {
            var news = await _context.News.FirstOrDefaultAsync(n => n.NewsId == request.NewsId, cancellationToken);
            if(news == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.NewsNotFound,
                    Message = Localizer["NewsNotFound"]
                });
            return new SearchIndividualNewsViewModel
            {
                News = _mapper.Map<NewsDto>(news)
            };
        }
    }
}