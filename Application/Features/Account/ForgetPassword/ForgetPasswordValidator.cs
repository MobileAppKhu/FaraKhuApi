﻿using Application.Resources;
using Application.Utilities;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.ForgetPassword
{
    public class ForgetPasswordValidator : AbstractValidator<ForgetPasswordCommand>
    {
        public ForgetPasswordValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .Must(e => e.IsEmail());
        }
    }
}