using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(notification => notification.NotificationId);
            builder.Property(notification => notification.NotificationId).ValueGeneratedOnAdd();
            builder.Property(notification => notification.Description).IsRequired();
            builder.Property(notification => notification.NotificationOperation).IsRequired();
            builder.Property(notification => notification.NotificationObjectType).IsRequired();
            builder.HasOne(notification => notification.User)
                .WithMany(user => user.Notifications)
                .HasForeignKey(notification => notification.UserId);
        }
    }
}