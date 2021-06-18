using System;
using System.Collections.Generic;
using Application.DTOs.Time;
using Domain.Models;
using MediatR;

namespace Application.Features.Course.Command.CreateCourse
{
    public class CreateCourseCommand : IRequest<CreateCourseViewModel>
    {
        public string CourseTitle { get; set; }
        public DateTime EndDate { get; set; }
    }
}