using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.Instructor;

public class SearchCourseInstructorDto : IMapFrom<Domain.Models.Instructor>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Models.Instructor, SearchCourseInstructorDto>();
    }
}