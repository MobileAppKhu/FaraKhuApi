using System.Data;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Event.Commands.AddEvent
{
    public class AddEventCommandValidator : AbstractValidator<AddEventCommand>
    {
        public AddEventCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(e => e.EventName)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
            RuleFor(e => e.EventTime)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}