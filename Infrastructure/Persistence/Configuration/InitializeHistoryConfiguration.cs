using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class InitializeHistoryConfiguration : IEntityTypeConfiguration<InitializeHistory>
{
        
    public void Configure(EntityTypeBuilder<InitializeHistory> builder)
    {
        builder.HasKey(s => s.Version);
        builder.Property(n => n.DateTime).HasDefaultValueSql("now() at time zone 'utc'");
    }
}