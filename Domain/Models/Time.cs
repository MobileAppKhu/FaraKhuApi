using System;

namespace Domain.Models
{
    public class Time : BaseEntity
    {
        public string TimeId { get; set; }
        
        public Course Course { get; set; }

        public string CourseId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}