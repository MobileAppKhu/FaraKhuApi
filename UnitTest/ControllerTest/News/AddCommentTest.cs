using System.Net;
using System.Threading.Tasks;
using Application.Features.News.Commands.AddComment;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.News
{
    public class AddCommentTest : AppFactory
    {
        
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/News/AddComment";

        public AddCommentTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task AddComment_Basic_ShouldWorkCorrectly()
        {
            // Assign
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new AddCommentCommand()
            {
                NewsId = "NewsId",
                Text = "Hello"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            var content = await response.GetContent();
            
            _outputHelper.WriteLine(content);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());            
        }
        
        [Fact]
        public async Task AddComment_Reply_ShouldWorkCorrectly()
        {
            // Assign
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new AddCommentCommand()
            {
                NewsId = "NewsId",
                Text = "Hello",
                ParentId = "CommentId"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            var content = await response.GetContent();
            
            _outputHelper.WriteLine(content);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());            
        }
        
        [Fact]
        public async Task AddComment_Reply_NewsShouldNotBeFound()
        {
            // Assign
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new AddCommentCommand()
            {
                NewsId = "NewsId",
                Text = "Hello",
                ParentId = "WrongParentId"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            var content = await response.GetContent();
            
            _outputHelper.WriteLine(content);
            
            // Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.CommentNotFound));            
        }
        
        [Fact]
        public async Task AddComment_Basic_NewsShouldNotBeFound()
        {
            // Assign
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new AddCommentCommand()
            {
                NewsId = "WrongNewsId",
                Text = "Hello"
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            var content = await response.GetContent();
            
            _outputHelper.WriteLine(content);
            
            // Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.NewsNotFound));            
        }
        
    }
}