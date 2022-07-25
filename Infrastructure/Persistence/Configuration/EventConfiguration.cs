using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.EventId);
        builder.Property(e => e.EventId).ValueGeneratedOnAdd();
        builder.Property(e => e.EventName).IsRequired();
        builder.Property(e => e.EventTime).IsRequired();
        builder.HasOne(e => e.User)
            .WithMany(u => u.Events)
            .HasForeignKey(e => e.UserId);
        builder.Property(user => user.CreatedDate).
            HasDefaultValueSql("now() at time zone 'utc'").ValueGeneratedOnAdd();
        builder.Property(user => user.LastModifiedDate).HasDefaultValueSql("now() at time zone 'utc'")
            .ValueGeneratedOnAddOrUpdate();
    }
}