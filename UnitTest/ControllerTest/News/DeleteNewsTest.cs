using System.Net;
using System.Threading.Tasks;
using Application.Features.News.Commands.DeleteNews;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.News
{
    public class DeleteNewsTest : AppFactory
    {
        private readonly string _path = "/api/News/DeleteNews";
        private readonly ITestOutputHelper _outputHelper;

        public DeleteNewsTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task DeleteNews_ShouldWorkCorrectly()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToPublicRelation();

            var data = new DeleteNewsCommand
            {
                NewsId = "DeleteNewsId"
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
        public async Task DeleteNews_NewsShouldNotBeFound()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToPublicRelation();

            var data = new DeleteNewsCommand
            {
                NewsId = "WrongNewsId"
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.NewsNotFound));
        }
    }
}