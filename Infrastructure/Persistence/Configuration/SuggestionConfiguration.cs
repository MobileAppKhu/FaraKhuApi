using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class SuggestionConfiguration : IEntityTypeConfiguration<Suggestion>
    {
        public void Configure(EntityTypeBuilder<Suggestion> builder)
        {
            builder.HasKey(s => s.SuggestionId);
            builder.Property(s => s.SuggestionId).ValueGeneratedOnAdd();
            builder.HasOne(s => s.Sender)
                .WithMany(user => user.Suggestions)
                .HasForeignKey(s => s.SenderId);
        }
    }
}