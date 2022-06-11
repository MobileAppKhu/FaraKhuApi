using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Application.DTOs.Student;
using Application.DTOs.Time;
using MediatR;

namespace Application.Features.Course.Commands.AddCourse
{
    public class AddCourseCommand : IRequest<AddCourseViewModel>
    {
        [JsonIgnore]
        public string UserId { get; set; }
        public String CourseTypeId{ get; set; }
        public string Address { get; set; }
        public List<AddTimeDto> AddTimeDtos { get; set; }
        public AddStudentDto AddStudentDto { get; set; }
        public DateTime EndDate { get; set; }
        public string AvatarId { get; set; }
        public string InstructorId { get; set; }
    }
}