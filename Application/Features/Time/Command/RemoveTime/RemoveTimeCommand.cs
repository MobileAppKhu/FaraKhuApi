using MediatR;

namespace Application.Features.Time.Command.RemoveTime
{
    public class RemoveTimeCommand : IRequest<Unit>
    {
        public string CourseId { get; set; }
        public string TimeId { get; set; }
    }
}