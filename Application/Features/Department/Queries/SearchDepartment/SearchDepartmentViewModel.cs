using System.Collections.Generic;
using Application.DTOs.Department;

namespace Application.Features.Department.Queries.SearchDepartment;

public class SearchDepartmentViewModel
{
    public List<DepartmentSearchDto> Departments { get; set; }
    public int SearchCount { get; set; }
}