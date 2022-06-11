using Domain.BaseModels;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class FileEntityConfiguration : IEntityTypeConfiguration<FileEntity>
    {
        public void Configure(EntityTypeBuilder<FileEntity> builder)
        {
            builder.HasKey(file => file.Id);
            builder.Property(file => file.Id).ValueGeneratedOnAdd();
            builder.Property(file => file.IsDeleted).HasDefaultValue(false);
            builder.Property(file => file.CreatedDate).HasDefaultValueSql("now() at time zone 'utc'")
                .ValueGeneratedOnAdd();
            builder.Property(file => file.LastModifiedDate)
                .HasDefaultValueSql("now() at time zone 'utc'")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}