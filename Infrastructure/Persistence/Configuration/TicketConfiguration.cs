using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(ticket => ticket.TicketId);
            builder.Property(ticket => ticket.TicketId).ValueGeneratedOnAdd();
            builder.HasOne(ticket => ticket.Creator)
                .WithMany(user => user.Tickets)
                .HasForeignKey(ticket => ticket.CreatorId);
            builder.Property(ticket => ticket.Description).IsRequired();
            builder.Property(ticket => ticket.Status).IsRequired();
            builder.Property(ticket => ticket.Priority).IsRequired();
            builder.Property(ticket => ticket.CreatedDate).HasDefaultValueSql("now() at time zone 'utc'")
                .ValueGeneratedOnAdd();
            builder.Property(ticket => ticket.LastModifiedDate)
                .HasDefaultValueSql("now() at time zone 'utc'")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}