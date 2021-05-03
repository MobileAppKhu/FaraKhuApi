using Application.Common.Mappings;
using AutoMapper;
using Domain.Enum;
using Domain.Models;

namespace Application.DTOs.User
{
    public class ProfileDto : IMapFrom<Domain.Models.Instructor>, IMapFrom<Domain.Models.Student>, IMapFrom<PROfficer>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Id { get; set; }

        public string UserType { get; set; }

        public string Email { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Student, ProfileDto>()
                .ForMember(s => s.Id,
                    opt =>
                        opt.MapFrom(src => src.StudentId))
                .ForMember(s => s.UserType,opt =>
                    opt.MapFrom(src => src.UserType.ToString()));
            profile.CreateMap<Domain.Models.Instructor, ProfileDto>()
                .ForMember(s => s.Id,
                    opt =>
                        opt.MapFrom(src => src.InstructorId))
                .ForMember(s => s.UserType,opt =>
                    opt.MapFrom(src => src.UserType.ToString()));
            profile.CreateMap<Domain.BaseModels.BaseUser, ProfileDto>()
                .ForMember(o => o.Id,
                    opt =>
                        opt.Ignore())
                .ForMember(o => o.UserType,
                    opt =>
                        opt.MapFrom(src => src.UserType.ToString()));
        }
    }
}