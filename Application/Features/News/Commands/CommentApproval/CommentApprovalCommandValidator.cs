using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Commands.CommentApproval;

public class CommentApprovalCommandValidator : AbstractValidator<CommentApprovalCommand>
{
    public CommentApprovalCommandValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(request => request.Status).IsInEnum().WithMessage(localizer["StatusEnum"]);
        RuleFor(request => request.CommentId).NotEmpty().WithMessage(localizer["CommentIdNotEmpty"]);
    }
}