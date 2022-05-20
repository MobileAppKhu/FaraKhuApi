using System.Net;
using System.Threading.Tasks;
using Application.Features.User.Queries.GetUserId;
using Application.Features.User.Queries.SearchProfile;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.User
{
    public class SearchProfileTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "/api/User/SearchProfile";

        public SearchProfileTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task SearchProfile_ShouldWorkCorrectly()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SearchProfileQuery
            {
                UserId = "StudentId"
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
        public async Task SearchProfile_UserShouldNotBeFound()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SearchProfileQuery
            {
                UserId = "WrongId"
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.UserNotFound));
        }
    }
}