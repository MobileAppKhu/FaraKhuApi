using System.Collections.Generic;
using Domain.BaseModels;
using Domain.Enum;

namespace Domain.Models
{
    public class Instructor : BaseUser
    {
        public string InstructorId { get; set; }
        
        public ICollection<Course> Courses { get; set; }
        public ICollection<PollQuestion> Polls { get; set; }
    }
}