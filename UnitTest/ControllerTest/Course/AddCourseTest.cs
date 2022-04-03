using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Application.DTOs.Student;
using Application.DTOs.Time;
using Application.Features.Course.Commands.AddCourse;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Course
{
    public class AddCourseTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Course/AddCourse";

        public AddCourseTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task AddCourse_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            
            string studentId = "12345";
            var addStudentDto = new AddStudentDto();
            addStudentDto.StudentIds = new List<string> {studentId};
            List<AddTimeDto> addTimeDtos = new List<AddTimeDto>();
            addTimeDtos.Add(new AddTimeDto
            {
                StartTime = "13-40",
                EndTime = "14-40",
                WeekDay = WeekDay.Saturday
            });
            
            var data = new AddCourseCommand
            {
                EndDate = DateTime.Now,
                AddStudentDto = addStudentDto,
                AddTimeDtos = addTimeDtos,
                CourseTypeId = "1",
                Address = "Test Address",
                AvatarId = "smiley.png"
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        }

        [Fact]
        public async Task AddCourse_InstructorCannotCreateCourseForAnotherInstructor()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new AddCourseCommand
            {
                EndDate = DateTime.Now,
                AddStudentDto = new AddStudentDto
                {
                    StudentIds = new List<string>()
                },
                AddTimeDtos = new List<AddTimeDto>(),
                CourseTypeId = "1",
                Address = "Address",
                AvatarId = "AvatarId",
                InstructorId = "SecondInstructor"
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode());
        }

        [Fact]
        public async Task AddCourse_OwnerCanCreateCourseForAnotherInstructor()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();
            var data = new AddCourseCommand
            {
                EndDate = DateTime.Now,
                AddStudentDto = new AddStudentDto
                {
                    StudentIds = new List<string>()
                },
                AddTimeDtos = new List<AddTimeDto>(),
                CourseTypeId = "1",
                Address = "Address",
                AvatarId = "smiley.png",
                InstructorId = "1234512345",
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        }

        [Fact]
        public async Task AddCourse_InstructorShouldNotBeFound()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();
            var data = new AddCourseCommand
            {
                EndDate = DateTime.Now,
                AddStudentDto = new AddStudentDto
                {
                    StudentIds = new List<string>()
                },
                AddTimeDtos = new List<AddTimeDto>(),
                CourseTypeId = "1",
                Address = "Address",
                AvatarId = "smiley.png",
                InstructorId = "FakeInstructorId",
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode());
        }
        [Fact]
        public async Task AddCourse_FileShouldNotBeFound()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new AddCourseCommand
            {
                EndDate = DateTime.Now,
                AddStudentDto = new AddStudentDto
                {
                    StudentIds = new List<string>()
                },
                AddTimeDtos = new List<AddTimeDto>(),
                CourseTypeId = "1",
                Address = "Address",
                AvatarId = "FakeFileId",
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode());
        }
        
        [Fact]
        public async Task AddCourse_TimeConflict()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new AddCourseCommand
            {
                EndDate = DateTime.Now,
                AddStudentDto = new AddStudentDto
                {
                    StudentIds = new List<string>()
                },
                AddTimeDtos = new List<AddTimeDto>
                {
                    new()
                    {
                        StartTime = "15-30",
                        EndTime = "17-30",
                        WeekDay = WeekDay.Monday
                    },
                    new()
                    {
                        StartTime = "16-30",
                        EndTime = "18-30",
                        WeekDay = WeekDay.Monday
                    }
                },
                CourseTypeId = "1",
                Address = "Address",
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode());
        }
    }
}