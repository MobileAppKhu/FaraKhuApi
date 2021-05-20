using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.BaseModels;

namespace Domain.Models
{
    public class Announcement : BaseEntity
    {
        public string AnnouncementId { get; set; }
        public string AnnouncementTitle { get; set; }
        public string AnnouncementDescription { get; set; }
        public string Department { get; set; }
        public string Faculty { get; set; }
        public BaseUser BaseUser { get; set; }
        public string UserId { get; set; }
        
    }
}
