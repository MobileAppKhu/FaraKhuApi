using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.News;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Commands.AddComment
{
    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Unit>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IMapper _mapper { get; }

        public AddCommentCommandHandler( IStringLocalizer<SharedResource> localizer,
            IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            Localizer = localizer;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = new Comment
            {
                UserId = request.UserId,
                Text = request.Text,
                ParentId = request.ParentId,
                Status = CommentStatus.Unapproved,
                NewsId = request.NewsId
            };
            await _context.Comments.AddAsync(comment, cancellationToken);
            var result = await _context.SaveChangesAsync(cancellationToken);
            if (result > 0) return Unit.Value;
            throw new CustomException(new Error()
            {
                ErrorType = ErrorType.Unexpected
            });
        }
    }
}