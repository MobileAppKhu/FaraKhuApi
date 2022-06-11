using System;
using System.Net;
using System.Threading.Tasks;
using Application.Features.CourseEvent.Commands.AddCourseEvent;
using Application.Features.CourseEvent.Commands.DeleteCourseEvent;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.CourseEvent
{
    public class DeleteCourseEventTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/CourseEvent/DeleteCourseEvent";

        public DeleteCourseEventTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task DeleteCourseEvent_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new DeleteCourseEventCommand()
            {
                CourseEventId = "1"
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
        public async Task DeleteCourseEvent_CourseEventShouldNotBeFound()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new DeleteCourseEventCommand()
            {
                CourseEventId = "9"
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.CourseEventNotFound));
        } 
        
        [Fact]
        public async Task DeleteCourseEvent_AnotherUserCantDeleteCourseEvent()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToSecondInstructor();

            var data = new DeleteCourseEventCommand()
            {
                CourseEventId = "2"
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.Unauthorized));
        } 
        
        [Fact]
        public async Task DeleteCourseEvent_AnotherUserTypeCantDeleteCourseEvent()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new DeleteCourseEventCommand()
            {
                CourseEventId = "2"
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.True(!await response.HasErrorCode(ErrorType.Unauthorized));
        }
    }
}