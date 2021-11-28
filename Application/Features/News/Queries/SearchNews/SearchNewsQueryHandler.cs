using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.News;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Queries.SearchNews
{
    public class SearchNewsQueryHandler : IRequestHandler<SearchNewsQuery, SearchNewsViewModel>
    {
        private readonly IDatabaseContext _context;
        private IMapper Mapper { get; }

        public SearchNewsQueryHandler(IMapper mapper, IDatabaseContext context)
        {
            _context = context;
            Mapper = mapper;
        }
        public async Task<SearchNewsViewModel> Handle(SearchNewsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Models.News> newsQueryable = _context.News;
            
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                newsQueryable = newsQueryable.Where(n => n.Title.ToLower().Contains(request.Search.ToLower()) || 
                                                         n.Description.ToLower().Contains(request.Search.ToLower()));
            }

            int searchCount = await newsQueryable.CountAsync(cancellationToken);
            List<Domain.Models.News> news = newsQueryable.Skip(request.Start).Take(request.Step).ToList();
            
            return new SearchNewsViewModel
            {
                News = Mapper.Map<List<NewsShortDto>>(news),
                SearchCount = searchCount
            };
        }
    }
}