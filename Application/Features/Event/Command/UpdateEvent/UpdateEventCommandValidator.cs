using FluentValidation;

namespace Application.Features.Event.Command.UpdateEvent
{
    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        public UpdateEventCommandValidator()
        {
            RuleFor(r => r.EventId)
                .NotEmpty();
            RuleFor(r => r.EventName)
                .NotEmpty();
            RuleFor(r => r.EventTime)
                .NotEmpty();
        }
    }
}