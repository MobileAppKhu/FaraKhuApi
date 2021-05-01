using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Command.AddNews
{
    public class AddNewsCommandValidator : AbstractValidator<AddNewsCommand>
    {
        public AddNewsCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.Title)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
            RuleFor(r => r.Description)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}