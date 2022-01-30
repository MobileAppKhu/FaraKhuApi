using System.Collections.Generic;
using Domain.BaseModels;
using Domain.Enum;

namespace Domain.Models
{
    public class Comment : BaseEntity
    {
        public Comment()
        {
            Parent = null;
        }
        public string CommentId { get; set; }
        public string Text { get; set; }
        public CommentStatus Status { get; set; }
        public string ParentId { get; set; }
        public string UserId { get; set; }
        public string NewsId { get; set; }
        public Comment Parent { get; set; }
        public BaseUser User { get; set; }
        public News News { get; set; }
        public ICollection<Comment> Replies { get; set; }
    }
}