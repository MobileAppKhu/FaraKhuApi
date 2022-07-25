using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.Department;

public class DepartmentSearchDto : IMapFrom<Domain.Models.Department>
{
    public string DepartmentId { get; set; }
    public string DepartmentTitle { get; set; }
    public string DepartmentCode { get; set; }
    public string FacultyId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Models.Department, DepartmentSearchDto>();
    }
}