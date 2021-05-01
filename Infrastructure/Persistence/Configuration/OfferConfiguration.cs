using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class OfferConfiguration : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.HasKey(o => o.OfferId);
            builder.Property(o => o.OfferId).ValueGeneratedOnAdd();
            builder.HasOne(o => o.BaseUser)
                .WithMany(u => u.Offers)
                .HasForeignKey(o => o.UserId);
        }
    }
}