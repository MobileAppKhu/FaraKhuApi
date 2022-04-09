using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using AutoMapper;
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
        private IMapper _mapper { get; }

        public RemoveCommentCommandHandler( IStringLocalizer<SharedResource> localizer, IDatabaseContext context, 
            IMapper mapper)
        {
            _context = context;
            Localizer = localizer;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(comment => comment.CommentId == request.CommentId, cancellationToken);

            if (comment.UserId != request.UserId)
            {
                throw new CustomException(new Error()
                {
                    ErrorType = ErrorType.Unauthorized
                });
            }

            _context.Comments.Remove(comment);
            var result = await _context.SaveChangesAsync(cancellationToken);
            if (result <= 0) throw new CustomException(new Error() {ErrorType = ErrorType.Unexpected});
            return Unit.Value;
        }
    }
}