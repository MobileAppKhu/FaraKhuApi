using Domain.BaseModels;

namespace Domain.Models
{
    public class Announcement : BaseEntity
    {
        public string AnnouncementId { get; set; }
        public string AnnouncementTitle { get; set; }
        public string AnnouncementDescription { get; set; }
        public BaseUser BaseUser { get; set; }
        public string UserId { get; set; }
        public FileEntity Avatar { get; set; }
        public string AvatarId { get; set; }
    }
}