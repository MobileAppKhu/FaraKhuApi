using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.Faculty;

public class FacultySearchDto : IMapFrom<Domain.Models.Faculty>
{
    public string FacultyId { get; set; }
    public string FacultyTitle { get; set; }
    public string FacultyCode { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Models.Faculty, FacultySearchDto>();
    }
}