using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Commands.RemoveComment
{
    public class RemoveCommentCommandValidator : AbstractValidator<RemoveCommentCommand>
    {
        public RemoveCommentCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(request => request.CommentId).NotEmpty()
                .WithMessage(localizer["CommentIdNotEmpty"]);
        }
    }
}