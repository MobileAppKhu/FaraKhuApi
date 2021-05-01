using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Command.RemoveNews
{
    public class RemoveNewsCommandValidator : AbstractValidator<RemoveNewsCommand>
    {
        public RemoveNewsCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.NewsId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}