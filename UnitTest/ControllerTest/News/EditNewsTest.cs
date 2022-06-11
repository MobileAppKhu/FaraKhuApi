using System.Net;
using System.Threading.Tasks;
using Application.Features.News.Commands.AddNews;
using Application.Features.News.Commands.EditNews;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.News
{
    public class EditNewsTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/News/EditNews";

        public EditNewsTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task EditNews_Basic_ShouldWorkCorrectly()
        {
            // Assign
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new EditNewsCommand
            {
                NewsId = "NewsId",
                Title = "Edit Title",
                Description = "Edit Description",
                FileId = "smiley.png",
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            var content = await response.GetContent();
            
            _outputHelper.WriteLine(content);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());            
        }
        
        [Theory]
        [InlineData("Student")]
        [InlineData("Instructor")]
        public async Task EditNews_BadAuth_ShouldWorkCorrectly(string actor)
        {
            // Assign
            var client = Host.GetTestClient();
            switch (actor)
            {
                case "Student":
                    await client.AuthToStudent();
                    break;
                case "Instructor":
                    await client.AuthToInstructor();
                    break;
            }

            await client.AuthToInstructor();

            var data = new EditNewsCommand
            { 
                NewsId = "NewsId",
                Title = "خبر جدید",
                Description = "به فراخو خوش آمدید",
                FileId = "smiley.png"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            var content = await response.GetContent();
            
            _outputHelper.WriteLine(content);
            
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task EditNews_WrongNewsId_NotAcceptableStatusCode()
        {
            // Assign
            var client = Host.GetTestClient();
            await client.AuthToOwner();
            var data = new EditNewsCommand
            {
                NewsId = "WrongNewsId",
                Title = "خبر جدید",
                Description = "به فراخو خوش آمدید",
                FileId = "smiley.png"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            
            // Assert
            
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode());            
            
        }
    }
}