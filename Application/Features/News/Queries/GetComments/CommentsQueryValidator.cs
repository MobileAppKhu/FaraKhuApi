using Domain.Enum;
using FluentValidation;

namespace Application.Features.News.Queries.GetComments;

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