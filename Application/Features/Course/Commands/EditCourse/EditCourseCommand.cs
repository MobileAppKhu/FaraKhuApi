using System.Collections.Generic;
using Application.DTOs.Student;
using Application.DTOs.Time;
using MediatR;

namespace Application.Features.Course.Commands.EditCourse
{
    public class EditCourseCommand : IRequest<Unit>
    {
        public string CourseId { get; set; }
        public AddStudentDto AddStudentDto { get; set; }
        public DeleteStudentDto DeleteStudentDto { get; set; }
        public List<AddTimeDto> AddTimeDtos { get; set; }
        public DeleteTimeDto DeleteTimeDto { get; set; }
    }
}