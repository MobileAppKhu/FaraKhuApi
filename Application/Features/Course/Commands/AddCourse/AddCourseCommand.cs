using System;
using System.Collections.Generic;
using Application.DTOs.Time;
using Domain.Models;
using MediatR;

namespace Application.Features.Course.Commands.AddCourse
{
    public class AddCourseCommand : IRequest<AddCourseViewModel>
    {
        public string CourseTitle { get; set; }
        public DateTime EndDate { get; set; }
    }
}