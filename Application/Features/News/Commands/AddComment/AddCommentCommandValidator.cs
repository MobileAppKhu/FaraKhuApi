using Application.Features.News.Commands.AddNews;
using Application.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Commands.AddComment
{
    public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
    {
        public AddCommentCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(request => request.Text).MaximumLength(200)
                .WithMessage(localizer["MaximumLength200"]);
            RuleFor(request => request.Text).MinimumLength(3)
                .WithMessage(localizer["MinimumLength3"]);
            RuleFor(request => request.NewsId).NotEmpty()
                .WithMessage(localizer["NewsIdNotEmpty"]);
        }
    }
}