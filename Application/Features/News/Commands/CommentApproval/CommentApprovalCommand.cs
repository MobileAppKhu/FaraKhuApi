using Domain.Enum;
using MediatR;

namespace Application.Features.News.Commands.CommentApproval;

public class CommentApprovalCommand 
    : IRequest<Unit>
{
    public CommentStatus Status { get; set; }
    public string CommentId { get; set; }
}