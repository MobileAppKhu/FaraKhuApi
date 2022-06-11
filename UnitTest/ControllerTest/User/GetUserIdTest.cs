using System.Net;
using System.Threading.Tasks;
using Application.Features.User.Queries.GetUserId;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.User
{
    public class GetUserIdTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "/api/User/GetUserId";

        public GetUserIdTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task GetUserId_ShouldWorkCorrectly()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new GetUserIdQuery();
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        }
    }
}