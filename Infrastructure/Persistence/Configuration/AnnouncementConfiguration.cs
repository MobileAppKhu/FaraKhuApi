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
            builder.HasOne(announcement => announcement.BaseUser)
                .WithMany(user => user.Announcements)
                .HasForeignKey(announcement => announcement.UserId);
            builder.HasOne(announcement => announcement.Avatar)
                .WithMany().HasForeignKey(announcement => announcement.AvatarId);
        }
    }
}