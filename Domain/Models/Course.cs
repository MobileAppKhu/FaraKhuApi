using System;
using System.Collections.Generic;
using Domain.BaseModels;
using Domain.Enum;

namespace Domain.Models
{
    public class Course : BaseEntity
    {
        public string CourseId { get; set; }
        public ICollection<Time> Times { get; set; }
        public ICollection<Student> Students { get; set; }
        public string InstructorId { get; set; }
        public Instructor Instructor { get; set; }
        public ICollection<CourseEvent> CourseEvents { get; set; }
        public ICollection<PollQuestion> Polls { get; set; }
        public DateTime EndDate { get; set; }
        public string CourseTypeId { get; set; }
        public CourseType CourseType { get; set; }
        public FileEntity Avatar { get; set; }
        public string AvatarId { get; set; }
    }
}