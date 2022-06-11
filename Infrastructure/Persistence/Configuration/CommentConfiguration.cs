using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(comment => comment.CommentId);
            builder.Property(comment => comment.CommentId).ValueGeneratedOnAdd();
            builder.Property(comment => comment.CreatedDate)
                .HasDefaultValueSql("now() at time zone 'utc'").ValueGeneratedOnAdd();
            builder.Property(comment => comment.LastModifiedDate)
                .HasDefaultValueSql("now() at time zone 'utc'")
                .ValueGeneratedOnAddOrUpdate();
            builder.HasOne(comment => comment.User)
                .WithMany(comment => comment.Comments)
                .HasForeignKey(comment => comment.UserId);
            builder.HasOne(comment => comment.News)
                .WithMany(news => news.Comments)
                .HasForeignKey(comment => comment.NewsId);
            builder.HasOne(comment => comment.Parent)
                .WithMany(comment => comment.Replies)
                .HasForeignKey(comment => comment.ParentId);
        }
    }
}