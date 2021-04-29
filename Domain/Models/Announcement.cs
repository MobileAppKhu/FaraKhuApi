using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Announcement : BaseEntity
    {
        public int AnnouncementId { get; set; }
        public string AnnouncementTitle { get; set; }
        public string AnnouncementDescription { get; set; }
        public string Department { get; set; }
        public string Faculty { get; set; }
        public Instructor Instructor { get; set; }
        public string InstructorId { get; set; }
        
    }
}
