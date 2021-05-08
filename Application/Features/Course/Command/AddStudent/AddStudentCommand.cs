using MediatR;

namespace Application.Features.Course.Command.AddStudent
{
    public class AddStudentCommand : IRequest<AddStudentViewModel>

    {
    public string CourseId { get; set; }
    public string StudentId { get; set; }
    }
}