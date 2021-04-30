using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Time.Command.AddTime
{
    public class AddTimeCommandValidator : AbstractValidator<AddTimeCommand>
    {
        public AddTimeCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.CourseId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
            RuleFor(r => r.StartTime)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
            RuleFor(r => r.EndTime)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}