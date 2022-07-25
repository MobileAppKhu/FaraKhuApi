using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.Student;

public class SearchCourseStudentDto : IMapFrom<Domain.Models.Student>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Models.Student, SearchCourseStudentDto>();
    }
}