using System.Net;
using System.Threading.Tasks;
using Application.Features.Announcement.Commands.AddAnnouncement;
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
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new AddAnnouncementCommand
            {
                Avatar = "smiley.png",
                Description = "Test Description",
                Title = "Test Title"
            };

            var response = await client.PostAsync(_path, data);

            _outputHelper.WriteLine(await response.GetContent());
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        }
    }
}