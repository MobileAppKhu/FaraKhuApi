using System.Net;
using System.Threading.Tasks;
using Application.Features.Notification.Commands.AddCourseNotification;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Notification
{
    public class AddCourseNotificationTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "/api/Notification/AddCourseNotification";

        public AddCourseNotificationTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task AddNotification_ShouldWorkCorrectly()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new AddCourseNotificationCommand
            {   
                Description = "New Notification",
                CourseId = "CourseId"
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
        public async Task AddNotification_ShouldBeUnauthorized()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToPublicRelation();

            var data = new AddCourseNotificationCommand
            {   
                Description = "New Notification",
                CourseId = "CourseId"
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
        public async Task AddNotification_CourseNotFound()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToPublicRelation();

            var data = new AddCourseNotificationCommand
            {   
                Description = "New Notification",
                CourseId = "WrongCourseId"
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.CourseNotFound));
        }

        [Fact]
        public async Task AddNotification_AnotherInstructorCannotCreateNotification()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToSecondInstructor();

            var data = new AddCourseNotificationCommand
            {   
                Description = "New Notification",
                CourseId = "CourseId"
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.Unauthorized));
        }
        
        
    }
}