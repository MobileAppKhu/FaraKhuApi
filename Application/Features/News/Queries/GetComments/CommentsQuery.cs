using Domain.Enum;
using MediatR;

namespace Application.Features.News.Queries.GetComments;

public class CommentsQuery : IRequest<CommentsQueryViewModel>
{
    public CommentQueryOption Option { get; set; }
    public string NewsId { get; set; }
    public bool OnlyUnapproved { get; set; }
    public int? Page { get; set; }
}