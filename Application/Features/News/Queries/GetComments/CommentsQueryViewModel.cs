﻿using System.Collections.Generic;
using Application.DTOs.News;
using Domain.Models;

namespace Application.Features.News.Queries.GetComments
{
    public class CommentsQueryViewModel
    {
        public List<Comment> Comments { get; set; }
    }
}