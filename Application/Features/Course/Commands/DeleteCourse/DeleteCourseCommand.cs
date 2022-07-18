using MediatR;

namespace Application.Features.Course.Commands.DeleteCourse;

public class DeleteCourseCommand : IRequest<Unit>
{
    public string UserId { get; set; }
    public string CourseId { get; set; }
}