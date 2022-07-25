using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Commands.DeleteNews;

public class DeleteNewsCommandValidator : AbstractValidator<DeleteNewsCommand>
{
    public DeleteNewsCommandValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(r => r.NewsId)
            .NotEmpty()
            .WithMessage(localizer["EmptyInput"]);
    }
}