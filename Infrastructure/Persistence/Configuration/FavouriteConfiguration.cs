using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
    {
        public void Configure(EntityTypeBuilder<Favourite> builder)
        {
            builder.HasKey(f => f.FavouriteId);
            builder.Property(f => f.FavouriteId).ValueGeneratedOnAdd();
            builder.HasOne(f => f.BaseUser)
                .WithMany(u => u.Favourites)
                .HasForeignKey(f => f.UserId);
            builder.Property(f => f.Description).IsRequired();
            builder.Property(f => f.CreatedDate).HasDefaultValueSql("now() at time zone 'utc'")
                .ValueGeneratedOnAdd();
            builder.Property(f => f.LastModifiedDate)
                .HasDefaultValueSql("now() at time zone 'utc'")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}