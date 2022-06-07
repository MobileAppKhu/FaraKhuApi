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

namespace Application.Features.News.Commands.RemoveComment
{
    public class RemoveCommentCommandHandler : IRequestHandler<RemoveCommentCommand, Unit>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }

        public RemoveCommentCommandHandler( IStringLocalizer<SharedResource> localizer, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
        }

        public async Task<Unit> Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.BaseUsers.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == request.CommentId, cancellationToken);

            if (comment == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.CommentNotFound,
                    Message = Localizer["CommentNotFound"]
                });
            }
            
            if (comment.UserId != request.UserId && user.UserType != UserType.Owner && user.UserType != UserType.PROfficer)
            {
                throw new CustomException(new Error()
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}