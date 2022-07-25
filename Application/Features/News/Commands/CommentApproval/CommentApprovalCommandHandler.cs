using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Commands.CommentApproval;

public class CommentApprovalCommandHandler : IRequestHandler<CommentApprovalCommand, Unit>
{
    private readonly IDatabaseContext _context;
    private IStringLocalizer<SharedResource> Localizer { get; }

    public CommentApprovalCommandHandler( IStringLocalizer<SharedResource> localizer,
        IDatabaseContext context)
    {
        _context = context;
        Localizer = localizer;
    }

    public async Task<Unit> Handle(CommentApprovalCommand request, CancellationToken cancellationToken)
    {
        var comment = await _context.Comments
            .FirstOrDefaultAsync(c => c.CommentId == request.CommentId, cancellationToken);
        if (comment == null)
        {
            throw new CustomException(new Error()
            {
                ErrorType = ErrorType.CommentNotFound,
                Message = Localizer["CommentNotFound"]
            });
        }

        comment.Status = request.Status;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}