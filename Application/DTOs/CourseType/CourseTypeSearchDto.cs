using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.CourseType;

public class CourseTypeSearchDto : IMapFrom<Domain.Models.CourseType>
{
    public string CourseTypeId { get; set; }
    public string CourseTypeTitle { get; set; }
    public string CourseTypeCode { get; set; }
    public string DepartmentId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Models.CourseType, CourseTypeSearchDto>();
    }
}