using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Ticket.Commands.DeleteTicket
{
    public class DeleteTicketCommandValidator : AbstractValidator<DeleteTicketCommand>
    {
        public DeleteTicketCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.TicketId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}