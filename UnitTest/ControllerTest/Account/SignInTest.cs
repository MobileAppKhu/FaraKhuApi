using System.Net;
using System.Threading.Tasks;
using Application.Features.Account.SignIn;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Account
{
    public class SignInTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Account/SignIn";

        public SignInTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task SignIn_ShouldWorkCorrectly()
        {
            
        }
        // [Fact]
        // public async Task SignIn_ShouldWorkCorrectly()
        // {
        //     // Arrange
        //     var client = Host.GetTestClient();
        //
        //     var data = new SignInCommand
        //     {
        //         Logon = "mehradmoshiri@khu.ac.ir",
        //         Password = "StudentPassword"
        //     };
        //
        //     //Act
        //     var response = await client.PostAsync(_path, data);
        //
        //     //Output
        //     _outputHelper.WriteLine(await response.GetContent());
        //
        //     //Assert
        //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //     Assert.True(!await response.HasErrorCode());
        // }
    }
}