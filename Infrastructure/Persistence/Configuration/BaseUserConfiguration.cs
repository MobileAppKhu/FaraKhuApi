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
            builder.Property(user => user.CreatedDate).
                HasDefaultValueSql("now() at time zone 'utc'").ValueGeneratedOnAdd();
            builder.Property(user => user.LastModifiedDate).HasDefaultValueSql("now() at time zone 'utc'")
                .ValueGeneratedOnAddOrUpdate();
            builder.Property(user => user.FirstName).IsRequired();
            builder.Property(user => user.LastName).IsRequired();
            
        }
    }
}