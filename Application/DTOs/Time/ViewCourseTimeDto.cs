using System;
using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.Time
{
    public class ViewCourseTimeDto : IMapFrom<Domain.Models.Time>
    {
        public string TimeId { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Time, ViewCourseTimeDto>();
        }
    }
}