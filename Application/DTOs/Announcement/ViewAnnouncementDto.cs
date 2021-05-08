using Application.Common.Mappings;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Announcement
{
    public class ViewAnnouncementDto : IMapFrom<Domain.Models.Announcement>
    {
        public string AnnouncementId { get; set; }
        public string AnnouncementTitle { get; set; }
        public string AnnouncementDescription { get; set; }
        public string Department { get; set; }
        public string Faculty { get; set; }
        
        public string InstructorFullname { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Announcement, ViewAnnouncementDto>()
                .ForMember(a => a.InstructorFullname,
                opt => opt.MapFrom(src => src.Instructor.FirstName + " " + src.Instructor.LastName));
        }
    }
}
