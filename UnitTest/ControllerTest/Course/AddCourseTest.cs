using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
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
            var endTime = DateTime.Now;
            
            var data = new AddCourseCommand
            {
                EndDate = endTime,
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
    }
}