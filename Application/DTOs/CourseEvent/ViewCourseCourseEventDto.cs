﻿using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Enum;

namespace Application.DTOs.CourseEvent
{
    public class ViewCourseCourseEventDto : IMapFrom<Domain.Models.CourseEvent>
    {
        public string EventName { get; set; }

        public DateTime EventTime { get; set; }

        public CourseEventType EventType { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.CourseEvent, ViewCourseCourseEventDto>();
        }
    }
}