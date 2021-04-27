﻿using System.Data;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Event.Command.CreateEvent
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(e => e.EventName)
                .NotEmpty();
            RuleFor(e => e.EventTime)
                .NotEmpty();
        }
    }
}