using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Commands.AddComment
{
    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }

        public AddCommentCommandHandler( IStringLocalizer<SharedResource> localizer,
            IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
        }

        public async Task<Unit> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var news = await _context.News.FirstOrDefaultAsync(n => n.NewsId == request.NewsId, cancellationToken);
            if (news == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.NewsNotFound,
                    Message = Localizer["NewsNotFound"]
                });
            }

            if (request.ParentId != null)
            {
                var parentComment =
                    await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == request.ParentId
                        && c.NewsId == request.NewsId,
                        cancellationToken);
                if (parentComment == null)
                {
                    throw new CustomException(new Error
                    {
                        ErrorType = ErrorType.CommentNotFound,
                        Message = Localizer["CommentNotFound"]
                    });
                }
            }
            
            var comment = new Comment
            {
                UserId = request.UserId,
                Text = request.Text,
                ParentId = request.ParentId,
                Status = CommentStatus.Unapproved,
                NewsId = request.NewsId
            };
            await _context.Comments.AddAsync(comment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}