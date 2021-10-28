using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Event.Commands.EditEvent
{
    public class EditEventCommandValidator : AbstractValidator<EditEventCommand>
    {
        public EditEventCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.EventId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
                
        }
    }
}