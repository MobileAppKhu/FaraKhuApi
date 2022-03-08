using System;
using System.Runtime.Serialization;
using Application.Common.Mappings;
using Application.DTOs.Announcement;
using Application.DTOs.Course;
using Application.DTOs.CourseEvent;
using Application.DTOs.CourseType;
using Application.DTOs.Department;
using Application.DTOs.Event.CourseEvent;
using Application.DTOs.Faculty;
using Application.DTOs.FileEntity;
using Application.DTOs.Instructor;
using Application.DTOs.News;
using Application.DTOs.Notification;
using Application.DTOs.Offer;
using Application.DTOs.Poll;
using Application.DTOs.Student;
using Application.DTOs.Ticket;
using Application.DTOs.Time;
using Application.DTOs.User;
using AutoMapper;
using Domain.BaseModels;
using Domain.Models;
using Xunit;

namespace UnitTest.MappingTest
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Theory]
        [InlineData(typeof(Announcement), typeof(SearchAnnouncementDto))]
        [InlineData(typeof(Course), typeof(SearchCourseDto))]
        [InlineData(typeof(Course), typeof(CourseShortDto))]
        [InlineData(typeof(CourseEvent), typeof(CourseEventShortDto))]
        [InlineData(typeof(CourseEvent), typeof(SearchCourseCourseEventDto))]
        [InlineData(typeof(CourseType), typeof(CourseTypeSearchDto))]
        [InlineData(typeof(Department), typeof(DepartmentSearchDto))]
        [InlineData(typeof(Faculty), typeof(FacultySearchDto))]
        [InlineData(typeof(FileEntity), typeof(DownloadDto))]
        [InlineData(typeof(Instructor), typeof(SearchCourseInstructorDto))]
        [InlineData(typeof(News), typeof(NewsDto))]
        [InlineData(typeof(Notification), typeof(NotificationSearchDto))]
        [InlineData(typeof(Offer), typeof(OfferDto))]
        [InlineData(typeof(Offer), typeof(UserOfferDto))]
        [InlineData(typeof(PollQuestion), typeof(PollQuestionDto))]
        [InlineData(typeof(PollQuestion), typeof(PollQuestionShortDto))]
        [InlineData(typeof(PollAnswer), typeof(PollAnswerDto))]
        [InlineData(typeof(Student), typeof(StudentShortDto))]
        [InlineData(typeof(Student), typeof(SearchCourseStudentDto))]
        [InlineData(typeof(Suggestion), typeof(Suggestion))]
        [InlineData(typeof(Ticket), typeof(TicketDto))]
        [InlineData(typeof(Time), typeof(SearchCourseTimeDto))]
        [InlineData(typeof(Time), typeof(SearchEventTimeDto))]
        [InlineData(typeof(Student), typeof(AllEventsDto))]
        [InlineData(typeof(Instructor), typeof(AllEventsDto))]
        [InlineData(typeof(Student), typeof(ProfileDto))]
        [InlineData(typeof(Instructor), typeof(ProfileDto))]
        [InlineData(typeof(Student), typeof(UserCoursesDto))]
        [InlineData(typeof(Instructor), typeof(UserCoursesDto))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = GetInstanceOf(source);

            _mapper.Map(instance, source, destination);
        }

        private object GetInstanceOf(Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(type);

            // Type without parameterless constructor
            return FormatterServices.GetUninitializedObject(type);
        }
    }
}