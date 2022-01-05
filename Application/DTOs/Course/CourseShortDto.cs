using System.Collections.Generic;
using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.Course
{
    public class CourseShortDto : IMapFrom<Domain.Models.CourseEvent>
    {
        public string CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string Address { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Course, CourseShortDto>();
        }
    }
}