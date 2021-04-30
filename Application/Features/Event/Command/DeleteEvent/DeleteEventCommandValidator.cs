
using FluentValidation;

namespace Application.Features.Event.Command.DeleteEvent
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