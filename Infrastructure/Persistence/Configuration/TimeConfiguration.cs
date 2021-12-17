using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class TimeConfiguration : IEntityTypeConfiguration<Time>
    {
        public void Configure(EntityTypeBuilder<Time> builder)
        {
            builder.HasKey(time => time.TimeId);
            builder.Property(time => time.TimeId).ValueGeneratedOnAdd();
            builder.HasOne(time => time.Course)
                .WithMany(course => course.Times)
                .HasForeignKey(time => time.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(time => time.StartTime).IsRequired();
            builder.Property(time => time.EndTime).IsRequired();
        }
    }
}