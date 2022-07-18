using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.Student;

public class StudentShortDto : IMapFrom<Domain.Models.Student>
{
    public string Id { get; set; }
    public string StudentId { get; set; }
    public string FullName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Models.Student, StudentShortDto>()
            .ForMember(d => d.FullName,
                opt =>
                    opt.MapFrom(src => src.FirstName + " " + src.LastName));
    }
}