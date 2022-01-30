using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace Application.Features.News.Queries.GetComments
{
    public class CommentsQueryHandler : IRequestHandler<CommentsQuery, CommentsQueryViewModel>
    {
        private readonly IDatabaseContext _context;
        private readonly IHttpContextAccessor _accessor;
        private IMapper Mapper { get; }

        public CommentsQueryHandler(IMapper mapper, IDatabaseContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
            Mapper = mapper;
        }

        public async Task<CommentsQueryViewModel> Handle(CommentsQuery request, CancellationToken cancellationToken)
        {
            List<Comment> comments;
            switch (request.Option)
            {
                case CommentQueryOption.All:
                    if (!_accessor.HttpContext.User.IsInRole("Owner"))
                        throw new CustomException(new Error()
                        {
                            ErrorType = ErrorType.Unauthorized,
                            Message = "You Can't See All Comments"
                        });
                    if (request.OnlyUnapproved)
                    {
                        comments = await _context.Comments.Where(comment => comment.Status == CommentStatus.Unapproved)
                            .ToListAsync(cancellationToken);
                    }
                    else
                    {
                        comments = await _context.Comments.ToListAsync(cancellationToken);
                    }
                    break;
                case CommentQueryOption.ByNews:
                    comments = await _context.Comments.Include(comment => comment.Replies).Where(comment => comment.NewsId == request.NewsId 
                            && comment.ParentId == null && comment.IsDeleted == false && comment.Status == CommentStatus.Approved).OrderByDescending(comment => comment.CreatedDate)
                        .ToListAsync(cancellationToken);
                    break;
                case CommentQueryOption.ByUser:
                    var userId = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    comments = await _context.Comments.Where(comment => comment.UserId == userId)
                        .ToListAsync(cancellationToken);
                    break;
                default:
                    throw new CustomException(new Error()
                    {
                        ErrorType = ErrorType.InvalidInput
                    });
            }

            if (request.Page != 0)
                comments.GetPage(request.Page.GetValueOrDefault());
            return new CommentsQueryViewModel()
            {
                Comments = comments
            };
        }
    }
}