using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Commands.EditNews
{
    public class EditNewsCommandValidator : AbstractValidator<EditNewsCommand>
    {
        public EditNewsCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.NewsId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
            RuleFor(r => r.Description);
        }
    }
}