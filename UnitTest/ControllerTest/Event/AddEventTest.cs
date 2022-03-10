using System;
using System.Net;
using System.Threading.Tasks;
using Application.Features.Event.Commands.AddEvent;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Event
{
    public class AddEventTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Event/AddEvent";
        
        public AddEventTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task AddEvent_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new AddEventCommand()
            {
                CourseId = "CourseId",
                EventDescription = "Description",
                EventName = "EventName",
                EventTime = DateTime.Now
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
        public async Task AddEvent_ShouldWorkCorrectlyForStudent()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new AddEventCommand()
            {
                CourseId = "CourseId",
                EventDescription = "Description",
                EventName = "EventName",
                EventTime = DateTime.Now
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
        public async Task AddEvent_ShouldNotWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new AddEventCommand()
            {
                CourseId = "NotCourseId",
                EventDescription = "Description",
                EventName = "EventName",
                EventTime = DateTime.Now
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
        public async Task AddEvent_ShouldNotCreateWithOutName()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new AddEventCommand()
            {
                CourseId = "CourseId",
                EventDescription = "Description",
                EventTime = DateTime.Now
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
        public async Task AddEvent_ShouldNotCreateWithOuttime()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new AddEventCommand()
            {
                CourseId = "CourseId",
                EventName = "EventName",
                EventDescription = "Description"
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