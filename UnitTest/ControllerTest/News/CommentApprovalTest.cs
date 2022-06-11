using System.Net;
using System.Threading.Tasks;
using Application.Features.News.Commands.CommentApproval;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.News
{
    public class CommentApprovalTest : AppFactory
    {
        private readonly string _path = "/api/News/CommentApproval";
        private readonly ITestOutputHelper _outputHelper;

        public CommentApprovalTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task CommentApproval_ShouldWorkCorrectly()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new CommentApprovalCommand
            {
                CommentId = "CommentId",
                Status = CommentStatus.Approved
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
        public async Task CommentApproval_CommentShouldNotBeFound()
        {
            //Assert
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new CommentApprovalCommand
            {
                CommentId = "WrongCommendId",
                Status = CommentStatus.Approved
            };

            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.CommentNotFound));
        }
    }
}