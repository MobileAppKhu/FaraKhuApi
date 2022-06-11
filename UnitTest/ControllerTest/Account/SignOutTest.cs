using System.Net;
using System.Threading.Tasks;
using Application.Features.Account.Commands.SignOut;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Account
{
    public class SignOutTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Account/SignOut";

        public SignOutTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task SignOut_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SignOutCommand();
            
            //Act
            var response = await client.PostAsync(_path, data);
        
            //Output
            _outputHelper.WriteLine(await response.GetContent());
        
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        }
        
        [Fact]
        public async Task SignOut_ShouldNotWork()
        {
            // Arrange
            var client = Host.GetTestClient();

            var data = new SignOutCommand();
            
            //Act
            var response = await client.PostAsync(_path, data);
        
            //Output
            _outputHelper.WriteLine(await response.GetContent());
        
            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}