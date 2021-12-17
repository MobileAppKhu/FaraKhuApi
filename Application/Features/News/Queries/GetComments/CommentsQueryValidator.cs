using Application.Features.News.Queries.SearchNews;
using Application.Resources;
using Domain.Enum;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Queries.GetComments
{
    public class CommentsQueryValidator : AbstractValidator<CommentsQuery>
    {
        public CommentsQueryValidator()
        {
            RuleFor(request => request.Option).IsInEnum();
            When(request => request.Option == CommentQueryOption.ByNews, () =>
            {
                RuleFor(request => request.NewsId).NotEmpty();
            });
            
            RuleFor(request => request.Page).NotEqual(0);
        }
    }
}