using System;
using System.Net;
using System.Threading.Tasks;
using Application.Features.CourseEvent.Commands.AddCourseEvent;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.CourseEvent
{
    public class AddCourseEventTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/CourseEvent/AddCourseEvent";

        public AddCourseEventTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task AddCourseEvent_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new AddCourseEventCommand
            {
                CourseId = "CourseId",
                EventDescription = "Description",
                EventName = "EventName",
                EventTime = DateTime.Now,
                EventType = CourseEventType.Assignment
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
        public async Task AddCourseEvent_ShouldBeUnauthorized()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new AddCourseEventCommand
            {
                CourseId = "CourseId",
                EventDescription = "Description",
                EventName = "EventName",
                EventTime = DateTime.Now,
                EventType = CourseEventType.Assignment
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        } 
        
        [Fact]
        public async Task AddCourseEvent_AnotherInstructorCantAddCourseEvent()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToSecondInstructor();

            var data = new AddCourseEventCommand
            {
                CourseId = "CourseId",
                EventDescription = "Description",
                EventName = "EventName",
                EventTime = DateTime.Now,
                EventType = CourseEventType.Assignment
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
        public async Task AddCourseEvent_CourseShouldNotBeFound()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new AddCourseEventCommand
            {
                CourseId = "Fail",
                EventDescription = "Description",
                EventName = "EventName",
                EventTime = DateTime.Now,
                EventType = CourseEventType.Assignment
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