using System;
using Application.DTOs.Instructor;
using Application.DTOs.Student;
using AutoMapper;
using Domain.Models;

namespace Application.Common.Mappings
{
    public class MappingConfig : Profile
    {
        public MappingConfig(IServiceProvider provider)
        {
            CreateMap<Instructor, InstructorProfileDto>();
            CreateMap<Student, StudentProfileDto>();
        }
    }
}