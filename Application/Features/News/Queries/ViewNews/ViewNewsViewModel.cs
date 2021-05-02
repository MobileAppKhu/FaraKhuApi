using System.Collections.Generic;
using Application.DTOs.News;

namespace Application.Features.News.Queries.ViewNews
{
    public class ViewNewsViewModel
    {
        public List<NewsShortDto> News { get; set; }
    }
}