using System;

namespace Domain.Models
{
    public class Time
    {
        public int TimeId { get; set; }
        
        public Course Course { get; set; }

        public int CourseId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}