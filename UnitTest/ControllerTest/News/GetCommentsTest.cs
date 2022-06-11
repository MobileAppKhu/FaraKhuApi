using System.Net;
using System.Threading.Tasks;
using Application.Features.News.Queries.GetComments;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.News
{
    public class GetCommentsTest : AppFactory
    {
        private readonly string _path = "/api/News/GetComments";
        private readonly ITestOutputHelper _outputHelper;

        public GetCommentsTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task GetComments_ShouldWorkCorrectlyByUser()
        {
            //Arrange
            var client = Host.GetTestClient();

            var data = new CommentsQuery
            {
                Option = CommentQueryOption.ByUser,
                OnlyUnapproved = true
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
        public async Task GetComments_ShouldWorkCorrectlyByNews()
        {
            //Arrange
            var client = Host.GetTestClient();

            var data = new CommentsQuery
            {
                Option = CommentQueryOption.ByNews,
                OnlyUnapproved = true,
                NewsId = "TestNewsId"
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
        public async Task GetComments_ShouldWorkCorrectlyAll()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();
            
            var data = new CommentsQuery
            {
                Option = CommentQueryOption.All
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
        public async Task GetComments_NormalUserCannotSeeAllComments()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();
            
            var data = new CommentsQuery
            {
                Option = CommentQueryOption.All,
                OnlyUnapproved = true
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