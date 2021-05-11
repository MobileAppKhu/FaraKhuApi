using System.Reflection;
using Domain.BaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class BaseUserConfiguration : IEntityTypeConfiguration<BaseUser>
    {
        public void Configure(EntityTypeBuilder<BaseUser> builder)
        {
            builder.HasKey(user => user.Id);
            builder.Property(user => user.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.UserName).HasDefaultValue("");
            builder.HasIndex(u => u.UserName).IsUnique().HasFilter("NOT \"UserName\"=''");
            builder.HasIndex(u => u.NormalizedUserName).IsUnique().HasFilter("NOT \"UserName\"=''");
            builder.Property(u => u.NormalizedUserName).HasDefaultValue("");
            builder.Property(user => user.CreatedDate).
                HasDefaultValueSql("now() at time zone 'utc'").ValueGeneratedOnAdd();
            builder.Property(user => user.LastModifiedDate).HasDefaultValueSql("now() at time zone 'utc'")
                .ValueGeneratedOnAddOrUpdate();
            builder.Property(user => user.FirstName).IsRequired();
            builder.Property(user => user.LastName).IsRequired();
            builder.HasOne(user => user.Avatar).WithMany().HasForeignKey(user => user.AvatarId);
        }
    }
}