using System;
using Application.DTOs.Instructor;
using Application.DTOs.Student;
using AutoMapper;
using Domain.Models;

namespace Application.Common.Mappings
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Instructor, InstructorProfileDto>()
                .ForMember(d => d.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(d => d.FirstName,
                    opt => opt.MapFrom(src => src.FirstName))
                .ForMember(d => d.LastName,
                    opt => opt.MapFrom(src => src.LastName));
            CreateMap<Student, StudentProfileDto>()
                .ForMember(d => d.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(d => d.FirstName,
                    opt => opt.MapFrom(src => src.FirstName))
                .ForMember(d => d.LastName,
                    opt => opt.MapFrom(src => src.LastName));;
        }
    }
}