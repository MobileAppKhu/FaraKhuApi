using System.Collections.Generic;
using Domain.BaseModels;
using Domain.Enum;

namespace Domain.Models
{
    public class Student : BaseUser
    {
        public string StudentId { get; set; }
        
        public UserType UserType { get; set; }

        public ICollection<Course> Courses { get; set; }
        
    }
}