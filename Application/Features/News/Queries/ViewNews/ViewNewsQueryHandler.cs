﻿using System.Collections.Generic;
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

namespace Application.Features.News.Queries.ViewNews
{
    public class ViewNewsQueryHandler : IRequestHandler<ViewNewsQuery, ViewNewsViewModel>
    {
        private readonly IDatabaseContext _context;
        private IMapper _mapper { get; }

        public ViewNewsQueryHandler(IMapper mapper, IDatabaseContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ViewNewsViewModel> Handle(ViewNewsQuery request, CancellationToken cancellationToken)
        {
            List<Domain.Models.News> news;
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                news = _context.News.Where(n => n.Title.ToLower().Contains(request.Search.ToLower()) || 
                                                n.Description.ToLower().Contains(request.Search.ToLower())).ToList();
            }
            else
            {
                news = _context.News.ToList();
            }

            int searchCount = news.Count;
            news = news.Skip(request.Start).Take(request.Step).ToList();
            
            return new ViewNewsViewModel
            {
                News = _mapper.Map<List<NewsShortDto>>(news),
                SearchCount = searchCount
            };
        }
    }
}