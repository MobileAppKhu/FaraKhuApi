﻿using System.Collections.Generic;

namespace Domain.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        public string CourseTitle { get; set; }

        public ICollection<Time> Times { get; set; }

        public ICollection<Student> Students { get; set; }

        public string InstructorId { get; set; }
        
        public Instructor Instructor { get; set; }
    }
}