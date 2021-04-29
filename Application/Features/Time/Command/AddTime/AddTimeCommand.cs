using MediatR;

namespace Application.Features.Time.Command.AddTime
{
    public class AddTimeCommand : IRequest<AddTimeViewModel>
    {
        public int CourseId { get; set; }
        
        public string StartTime { get; set; }

        public string EndTime { get; set; }
    }
}