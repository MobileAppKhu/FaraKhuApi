using System;
using System.Collections.Generic;
using Application.DTOs.Student;
using Application.DTOs.Time;
using Domain.Enum;
using Domain.Models;
using MediatR;

namespace Application.Features.Course.Commands.AddCourse
{
    public class AddCourseCommand : IRequest<AddCourseViewModel>
    {
        public String CourseTypeId{ get; set; }
        public List<AddTimeDto> AddTimeDtos { get; set; }
        public AddStudentDto AddStudentDto { get; set; }
        public DateTime EndDate { get; set; }
        public string AvatarId { get; set; }
    }
}