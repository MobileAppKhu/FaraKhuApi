using System.Net;
using System.Threading.Tasks;
using Application.Features.Announcement.Commands.DeleteAnnouncement;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Announcement
{
    public class DeleteAnnouncementTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Announcement/DeleteAnnouncement";

        public DeleteAnnouncementTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task DeleteAnnouncement_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();
            
            var data = new DeleteAnnouncementCommand
            {
                AnnouncementId = "DeleteAnnouncement"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            
            // Output
            _outputHelper.WriteLine(await response.GetContent());
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        }

        [Fact]
        public async Task DeleteAnnouncement_ShouldBeUnauthorized()
        {
            // Arrange
            var client = Host.GetTestClient();
            
            var data = new DeleteAnnouncementCommand
            {
                AnnouncementId = "DeleteAnnouncement"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            
            // Output
            _outputHelper.WriteLine(await response.GetContent());
            
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAnnouncement_AnotherUserCannotDelete()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            
            var data = new DeleteAnnouncementCommand
            {
                AnnouncementId = "EditAnnouncement"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            
            // Output
            _outputHelper.WriteLine(await response.GetContent());
            
            // Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.Unauthorized));
        }

        [Fact]
        public async Task DeleteAnnouncement_OwnerCanDelete()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();
            
            var data = new DeleteAnnouncementCommand
            {
                AnnouncementId = "DeleteAnnouncement1"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            
            // Output
            _outputHelper.WriteLine(await response.GetContent());
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        }
        
        [Fact]
        public async Task DeleteAnnouncement_AnnouncementShouldNotBeFound()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();
            
            var data = new DeleteAnnouncementCommand
            {
                AnnouncementId = "WrongAnnouncementId"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            
            // Output
            _outputHelper.WriteLine(await response.GetContent());
            
            // Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.AnnouncementNotFound));
        }
    }
}