using Domain.Enum;
using MediatR;

namespace Application.Features.Department.Queries.SearchDepartment;

public class SearchDepartmentQuery : IRequest<SearchDepartmentViewModel>
{
    public string DepartmentId { get; set; }
    public string DepartmentTitle { get; set; }
    public string DepartmentCode { get; set; }
    public string FacultyId { get; set; }
    public int Start { get; set; }
    public int Step{ get; set; }
    public DepartmentColumn DepartmentColumn { get; set; }
    public bool OrderDirection { get; set; }
}