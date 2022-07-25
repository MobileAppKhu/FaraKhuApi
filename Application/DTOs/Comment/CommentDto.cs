using System.Collections.Generic;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Enum;

namespace Application.DTOs.Comment;

public class CommentDto : IMapFrom<Domain.Models.Comment>
{
    public string CommentId { get; set; }
    public string Text { get; set; }
    public CommentStatus Status { get; set; }
    public string ParentId { get; set; }
    public string UserId { get; set; }
    public string NewsId { get; set; }
    public ICollection<CommentDto> Replies { get; set; }
        
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Models.Comment, CommentDto>();
    }
}