using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.HasKey(n => n.NewsId);
            builder.Property(n => n.NewsId).ValueGeneratedOnAdd();
            builder.Property(n => n.Title).IsRequired();
            builder.HasOne(n => n.FileEntity).WithMany().HasForeignKey(n => n.FileId);
        }
    }
}