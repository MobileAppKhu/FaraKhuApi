using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.File.Commands.Upload;

public class UploadCommandValidator : AbstractValidator<UploadCommand>
{
    public UploadCommandValidator(IStringLocalizer<SharedResource> localizer)
    {
    }
}