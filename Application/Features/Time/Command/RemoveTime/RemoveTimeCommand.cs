using MediatR;

namespace Application.Features.Time.Command.RemoveTime
{
    public class RemoveTimeCommand : IRequest<Unit>
    {
        public int CourseId { get; set; }
        public int TimeId { get; set; }
    }
}