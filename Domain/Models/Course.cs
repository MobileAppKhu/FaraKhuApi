using System.Collections.Generic;

namespace Domain.Models
{
    public class Course : BaseEntity
    {
        public string CourseId { get; set; }

        public string CourseTitle { get; set; }

        public ICollection<Time> Times { get; set; }

        public ICollection<Student> Students { get; set; }

        public string InstructorId { get; set; }
        
        public Instructor Instructor { get; set; }

        public ICollection<CourseEvent> CourseEvents { get; set; }
        public ICollection<PollQuestion> Polls { get; set; }
    }
}