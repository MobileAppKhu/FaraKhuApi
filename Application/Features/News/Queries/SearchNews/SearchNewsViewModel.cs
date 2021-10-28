using System.Collections.Generic;
using Application.DTOs.News;

namespace Application.Features.News.Queries.SearchNews
{
    public class SearchNewsViewModel
    {
        public List<NewsShortDto> News { get; set; }
        public int SearchCount { get; set; }
    }
}