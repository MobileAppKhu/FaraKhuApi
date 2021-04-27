using Domain.Enum;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(student => student.StudentId).IsRequired();
            builder.HasMany(student => student.Courses).
                WithMany(course => course.Students);
            builder.Property(student => student.UserType).HasDefaultValue(UserType.Student);
        }
    }
}