using Domain.Enum;
using MediatR;

namespace Application.Features.Time.Command.AddTime
{
    public class AddTimeCommand : IRequest<AddTimeViewModel>
    {
        public string CourseId { get; set; }
        public WeekDay WeekDay { get; set; }
        public string StartTime { get; set; }
        //hh-mm
        public string EndTime { get; set; }
    }
}