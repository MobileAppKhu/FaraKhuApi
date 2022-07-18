using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.File.Queries.Download;

public class DownloadQueryValidator : AbstractValidator<DownloadQuery>
{
    public DownloadQueryValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(r => r.FileId)
            .NotEmpty()
            .WithMessage(localizer["InvalidFileID"]);
    }
}