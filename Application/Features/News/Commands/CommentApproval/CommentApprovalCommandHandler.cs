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

namespace Application.Features.News.Commands.CommentApproval
{
    public class CommentApprovalCommandHandler : IRequestHandler<CommentApprovalCommand, Unit>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IMapper _mapper { get; }

        public CommentApprovalCommandHandler( IStringLocalizer<SharedResource> localizer,
            IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            Localizer = localizer;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CommentApprovalCommand request, CancellationToken cancellationToken)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(comment => comment.CommentId == request.CommentId, cancellationToken);
            if (comment == null)
                throw new CustomException(new Error()
                {
                    ErrorType = ErrorType.CommentNotFound,
                    Message = "Comment Not Found"
                });
            comment.Status = request.Status;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}