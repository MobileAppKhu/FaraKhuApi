using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(e => e.DepartmentId);
            builder.Property(e => e.DepartmentTitle).IsRequired();
            builder.Property(e => e.DepartmentCode).IsRequired();
            builder.HasOne(e => e.Faculty)
                .WithMany(f => f.Departments)
                .HasForeignKey(e => e.FacultyId);
        }
    }
}