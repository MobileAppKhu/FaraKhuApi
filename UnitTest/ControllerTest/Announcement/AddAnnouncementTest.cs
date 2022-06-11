using System.Net;
using System.Threading.Tasks;
using Application.Features.Announcement.Commands.AddAnnouncement;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Announcement
{
    public class AddAnnouncementTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Announcement/AddAnnouncement";

        public AddAnnouncementTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task AddAnnouncement_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new AddAnnouncementCommand
            {
                Avatar = "smiley.png",
                Description = "Test Description",
                Title = "Test Title"
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
        public async Task AddAnnouncement_ShouldBeUnauthorized()
        {
            // Arrange
            var client = Host.GetTestClient();
            var data = new AddAnnouncementCommand
            {
                Avatar = "smiley.png",
                Description = "Test Description",
                Title = "Test Title"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            
            // Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task AddAnnouncement_FileShouldNotBeFound()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new AddAnnouncementCommand
            {
                Avatar = "WrongFileId",
                Description = "Test Description",
                Title = "Test Title"
            };

            // Act
            var response = await client.PostAsync(_path, data);

            // Output
            _outputHelper.WriteLine(await response.GetContent());
            
            // Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.FileNotFound));
        }
        
        [Fact]
        public async Task AddAnnouncement_ShouldBeWorkingWithoutAvatar()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new AddAnnouncementCommand
            {
                Description = "Test Description",
                Title = "Test Title"
            };

            // Act
            var response = await client.PostAsync(_path, data);

            // Output
            _outputHelper.WriteLine(await response.GetContent());
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        }
    }
}