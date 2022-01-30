using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(course => course.CourseId);
            builder.Property(course => course.CourseId).ValueGeneratedOnAdd();
            builder.HasMany(course => course.Students)
                .WithMany(student => student.Courses);
            builder.HasOne(course => course.Instructor)
                .WithMany(instructor => instructor.Courses)
                .HasForeignKey(course => course.InstructorId);
            builder.HasOne(course => course.CourseType).WithMany().HasForeignKey(course => course.CourseTypeId);
            builder.HasOne(course => course.Avatar)
                .WithMany().HasForeignKey(course => course.AvatarId);
        }
    }
}