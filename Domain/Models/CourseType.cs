using System;

namespace Domain.Models
{
    public class CourseType
    {
        public string CourseTypeId { get; set; }
        public string CourseTypeTitle { get; set; }
        public string CourseTypeCode { get; set; }
        public string DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}