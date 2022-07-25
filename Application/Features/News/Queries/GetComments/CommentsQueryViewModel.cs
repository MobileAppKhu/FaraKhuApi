using System.Collections.Generic;
using Application.DTOs.Comment;

namespace Application.Features.News.Queries.GetComments;

public class CommentsQueryViewModel
{
    public List<CommentDto> Comments { get; set; }
}