using Domain.Enum;

namespace Application.DTOs.Time
{
    public class EditTimeDto
    {
        public string TimeId { get; set; }
        public WeekDay WeekDay { get; set; }

        public string StartTime { get; set; }

        //hh-mm
        public string EndTime { get; set; }
    }
}