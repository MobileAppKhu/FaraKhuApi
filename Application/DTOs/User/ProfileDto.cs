using Application.Common.Mappings;
using Application.DTOs.Student;
using AutoMapper;
using Domain.Enum;

namespace Application.DTOs.User
{
    public class ProfileDto : IMapFrom<Domain.Models.Instructor>, IMapFrom<Domain.Models.Student>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Id { get; set; }

        public UserType UserType { get; set; }

        public string Email { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Student, ProfileDto>()
                .ForMember(s => s.Id,
                    opt =>
                        opt.MapFrom(src => src.StudentId))
                .ForMember(s => s.UserType,opt =>
                    opt.MapFrom(src => src.UserType));
            profile.CreateMap<Domain.Models.Instructor, ProfileDto>()
                .ForMember(s => s.Id,
                    opt =>
                        opt.MapFrom(src => src.InstructorId))
                .ForMember(s => s.UserType,opt =>
                    opt.MapFrom(src => src.UserType));
        }
    }
}