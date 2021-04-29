using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configuration
{
    public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
    {
        public void Configure(EntityTypeBuilder<Announcement> builder)
        {
            builder.HasKey(announcement => announcement.AnnouncementId);
            builder.Property(announcement => announcement.AnnouncementId).ValueGeneratedOnAdd();
            builder.HasOne(announcement => announcement.Instructor)
                .WithMany(instructor => instructor.Announcements)
                .HasForeignKey(announcement => announcement.InstructorId);
        }
    }
}
