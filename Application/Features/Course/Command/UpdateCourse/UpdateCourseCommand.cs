﻿using MediatR;

namespace Application.Features.Course.Command.UpdateCourse
{
    public class UpdateCourseCommand : IRequest<Unit>
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
    }
}