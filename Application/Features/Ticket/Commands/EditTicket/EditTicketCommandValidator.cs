using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Ticket.Commands.EditTicket
{
    public class EditTicketCommandValidator : AbstractValidator<EditTicketCommand>
    {
        public EditTicketCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.TicketId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}