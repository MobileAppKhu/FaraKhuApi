using System.Net;
using System.Threading.Tasks;
using Application.Features.Announcement.Commands.EditAnnouncement;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Announcement
{
    public class EditAnnouncementTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Announcement/EditAnnouncement";

        public EditAnnouncementTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task EditAnnouncement_DescriptionShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();
            
            var data = new EditAnnouncementCommand
            {
                AnnouncementId = "EditAnnouncement",
                Description = "New Description"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            
            // Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        }

        [Fact]
        public async Task EditAnnouncement_TitleShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();
            
            var data = new EditAnnouncementCommand
            {
                AnnouncementId = "EditAnnouncement",
                Title = "New Title"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            
            // Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        }

        [Fact]
        public async Task EditAnnouncement_AvatarShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();
            
            var data = new EditAnnouncementCommand
            {
                AnnouncementId = "EditAnnouncement",
                AvatarId = "sad.png"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            
            // Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        }
        
        [Fact]
        public async Task EditAnnouncement_FileShouldNotBeFound()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();
            
            var data = new EditAnnouncementCommand
            {
                AnnouncementId = "EditAnnouncement",
                AvatarId = "WrongFileId"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            
            // Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.FileNotFound));
        }
        
        [Fact]
        public async Task EditAnnouncement_AnotherUserCannotEdit()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToSecondStudent();
            
            var data = new EditAnnouncementCommand
            {
                AnnouncementId = "EditAnnouncement",
                Title = "New Title"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            
            // Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.Unauthorized));
        }
        
        [Fact]
        public async Task EditAnnouncement_OwnerCanEdit()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();
            
            var data = new EditAnnouncementCommand
            {
                AnnouncementId = "EditAnnouncement",
                Title = "New Title1"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            
            // Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        }
        
        [Fact]
        public async Task EditAnnouncement_AnnouncementShouldNotBeFound()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();
            
            var data = new EditAnnouncementCommand
            {
                AnnouncementId = "WrongAnnouncementId",
                Title = "New Title1"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            
            // Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.AnnouncementNotFound));
        }
    }
}