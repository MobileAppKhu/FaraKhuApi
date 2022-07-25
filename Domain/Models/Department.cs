using System.Collections.Generic;

namespace Domain.Models;

public class Department : BaseEntity
{
    public string DepartmentId { get; set; }
    public string DepartmentTitle { get; set; }
    public string DepartmentCode { get; set; }
    public string FacultyId { get; set; }
    public Faculty Faculty { get; set; }
    public ICollection<CourseType> CourseTypes { get; set; }

}