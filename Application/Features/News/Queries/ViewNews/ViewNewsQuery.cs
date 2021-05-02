﻿using MediatR;

namespace Application.Features.News.Queries.ViewNews
{
    public class ViewNewsQuery : IRequest<ViewNewsViewModel>
    {
        public string Search { get; set; }
    }
}