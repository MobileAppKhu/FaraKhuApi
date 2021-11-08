using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Faculty
    {
        public string FacultyId { get; set; }
        public string FacultyTitle { get; set; }
        public string FacultyCode { get; set; }
        public ICollection<Department> Departments { get; set; }
    }
}