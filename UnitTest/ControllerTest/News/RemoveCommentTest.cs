using System.Net;
using System.Threading.Tasks;
using Application.Features.News.Commands.RemoveComment;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.News
{
    public class RemoveCommentTest : AppFactory
    {
        private readonly string _path = "/api/News/RemoveComment";
        private readonly ITestOutputHelper _outputHelper;

        public RemoveCommentTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task RemoveComment_ShouldWorkCorrectly()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new RemoveCommentCommand
            {
                CommentId = "RemoveCommentId"
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
        public async Task RemoveComment_CommentShouldNotBeFound()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new RemoveCommentCommand
            {
                CommentId = "WrongCommentId"
            };

            //Act
            var response = await client.PostAsync(_path, data);

            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.CommentNotFound));
        }

        [Fact]
        public async Task RemoveComment_AnotherUserCannotDeleteComment()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToSecondStudent();

            var data = new RemoveCommentCommand
            {
                CommentId = "CommentId"
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