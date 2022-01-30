using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Ticket.Commands.AddTicket
{
    public class AddTicketCommandValidator : AbstractValidator<AddTicketCommand>
    {
        public AddTicketCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.Description)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
            RuleFor(r => r.Priority)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}