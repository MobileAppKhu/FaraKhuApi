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
            builder.Property(ce => ce.CourseId).ValueGeneratedOnAdd();
            builder.HasOne(ce => ce.Course)
                .WithMany(c => c.CourseEvents)
                .HasForeignKey(ce => ce.CourseEventId);
            //TODO
        }
    }
}