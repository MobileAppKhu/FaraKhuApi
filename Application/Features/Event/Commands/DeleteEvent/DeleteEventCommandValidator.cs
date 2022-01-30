
using FluentValidation;

namespace Application.Features.Event.Commands.DeleteEvent
{
    public class DeleteEventValidator : AbstractValidator<DeleteEventCommand>
    {
        public DeleteEventValidator()
        {
            RuleFor(r => r.EventId)
                .NotEmpty();
        }
    }
}