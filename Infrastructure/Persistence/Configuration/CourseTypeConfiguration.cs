using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class CourseTypeConfiguration : IEntityTypeConfiguration<CourseType>
    {
        public void Configure(EntityTypeBuilder<CourseType> builder)
        {
            builder.HasKey(e => e.CourseTypeId);
            builder.Property(e => e.CourseTypeId).ValueGeneratedOnAdd();
            builder.Property(e => e.CourseTypeCode).IsRequired();
            builder.Property(e => e.CourseTypeTitle).IsRequired();
            builder.HasOne(e => e.Department)
                .WithMany(d => d.CourseTypes)
                .HasForeignKey(e => e.DepartmentId);
        }
    }
}