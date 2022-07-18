using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Application.DTOs.Student;
using Application.DTOs.Time;
using MediatR;

namespace Application.Features.Course.Commands.EditCourse;

public class EditCourseCommand : IRequest<Unit>
{
    [JsonIgnore]
    public string UserId { get; set; }
    public string CourseId { get; set; }
    public string Address { get; set; }
    public DateTime? EndDate { get; set; }
    public string CourseTypeId { get; set; }
    public string AvatarId { get; set; }
    public AddStudentDto AddStudentDto { get; set; }
    public DeleteStudentDto DeleteStudentDto { get; set; }
    public List<AddTimeDto> AddTimeDtos { get; set; }
    public DeleteTimeDto DeleteTimeDto { get; set; }
}