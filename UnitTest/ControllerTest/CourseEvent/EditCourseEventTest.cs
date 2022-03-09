using System;
using System.Net;
using System.Threading.Tasks;
using Application.Features.CourseEvent.Commands.DeleteCourseEvent;
using Application.Features.CourseEvent.Commands.EditCourseEvent;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;


namespace UnitTest.ControllerTest.CourseEvent
{
    public class EditCourseEventTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/CourseEvent/EditCourseEvent";
        
        public EditCourseEventTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task EditCourseEventDescription_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new EditCourseEventCommand()
            {
                CourseEventId = "1",
                Description = "EditedDescription",
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
        public async Task EditCourseEventName_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new EditCourseEventCommand()
            {
                CourseEventId = "1",
                EventName = "EditedEventName",
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
        public async Task EditCourseEventType_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new EditCourseEventCommand()
            {
                CourseEventId = "1",
                CourseEventType = CourseEventType.Exam,
                
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
        public async Task EditCourseEventTime_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new EditCourseEventCommand()
            {
                CourseEventId = "1",
                EventTime = DateTime.Today.ToString()
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
        public async Task EditCourseEvent_CourseEventShouldNotBeFound()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new EditCourseEventCommand()
            {
                Description = "EditedDescription",
                EventName = "EditedEventName",
                CourseEventId = "2",
                CourseEventType = CourseEventType.Assignment
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
        public async Task EditCourseEvent_AnotherInstructorCantEditCourseEvent()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToSecondInstructor();

            var data = new EditCourseEventCommand()
            {
                Description = "EditedDescription",
                EventName = "EditedEventName",
                CourseEventId = "1",
                CourseEventType = CourseEventType.Assignment
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
        public async Task EditCourseEvent_ShouldBeUnauthorized()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new EditCourseEventCommand()
            {
                Description = "EditedDescription",
                EventName = "EditedEventName",
                CourseEventId = "1",
                CourseEventType = CourseEventType.Assignment
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        }
        
    }
}