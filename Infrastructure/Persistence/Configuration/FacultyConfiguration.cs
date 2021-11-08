using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class FacultyConfiguration : IEntityTypeConfiguration<Faculty>
    {
        public void Configure(EntityTypeBuilder<Faculty> builder)
        {
            builder.HasKey(e => e.FacultyId);
            builder.Property(e => e.FacultyId).ValueGeneratedOnAdd();
            builder.Property(e => e.FacultyCode).IsRequired();
            builder.Property(e => e.FacultyTitle).IsRequired();
        }
    }
}