using System;
using System.Net;
using System.Threading.Tasks;
using Application.Features.Notification.Commands.DeleteNotification;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Notification
{
    public class DeleteNotificationTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "/api/Notification/DeleteNotification";

        public DeleteNotificationTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task DeleteNotification_ShouldWorkCorrectly()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new DeleteNotificationCommand
            {
                NotificationId = "DeleteNotificationId"
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
        public async Task DeleteNotification_NotificationShouldNotBeFound()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new DeleteNotificationCommand
            {
                NotificationId = "WrongNotificationId"
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable,response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.NotificationNotFound));
        }
    }
}