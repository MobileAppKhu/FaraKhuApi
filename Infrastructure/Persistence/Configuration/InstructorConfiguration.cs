using Domain.Enum;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.Property(instructor => instructor.InstructorId).IsRequired();
            builder.Property(instructor  => instructor.UserType).HasDefaultValue(UserType.Instructor);

        }
    }
}