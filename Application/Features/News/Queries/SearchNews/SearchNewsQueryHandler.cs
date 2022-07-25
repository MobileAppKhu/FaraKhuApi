using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.News;
using AutoMapper;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.News.Queries.SearchNews;

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

        switch (request.NewsColumn)
        {
            case NewsColumn.NewsId:
                newsQueryable = request.OrderDirection
                    ? newsQueryable.OrderBy(news => news.NewsId)
                    : newsQueryable.OrderByDescending(news => news.NewsId);
                break;
            case NewsColumn.Title:
                newsQueryable = request.OrderDirection
                    ? newsQueryable.OrderBy(news => news.Title).ThenBy(news => news.NewsId)
                    : newsQueryable.OrderByDescending(news => news.Title).ThenByDescending(news => news.NewsId);
                break;
            case NewsColumn.Description:
                newsQueryable = request.OrderDirection
                    ? newsQueryable.OrderBy(news => news.Description).ThenBy(news => news.NewsId)
                    : newsQueryable.OrderByDescending(news => news.Description)
                        .ThenByDescending(news => news.NewsId);
                break;
            case NewsColumn.CreationDate:
                newsQueryable = request.OrderDirection
                    ? newsQueryable.OrderBy(news => news.CreatedDate).ThenBy(news => news.NewsId)
                    : newsQueryable.OrderByDescending(news => news.CreatedDate)
                        .ThenByDescending(news => news.NewsId);
                break;
        }

        int searchCount = await newsQueryable.CountAsync(cancellationToken);
        List<Domain.Models.News> newsList = newsQueryable.Skip(request.Start).Take(request.Step).ToList();
            
        return new SearchNewsViewModel
        {
            News = Mapper.Map<List<NewsDto>>(newsList),
            SearchCount = searchCount
        };
    }
}