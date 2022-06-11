using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class CourseEventConfiguration : IEntityTypeConfiguration<CourseEvent>
    {
        public void Configure(EntityTypeBuilder<CourseEvent> builder)
        {
            builder.HasKey(ce => ce.CourseEventId);
            builder.Property(ce => ce.CourseEventId).ValueGeneratedOnAdd();
            builder.HasOne(ce => ce.Course)
                .WithMany(c => c.CourseEvents)
                .HasForeignKey(ce => ce.CourseId);
            builder.Property(ce => ce.CreatedDate).HasDefaultValueSql("now() at time zone 'utc'")
                .ValueGeneratedOnAdd();
            builder.Property(ce => ce.LastModifiedDate)
                .HasDefaultValueSql("now() at time zone 'utc'")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}