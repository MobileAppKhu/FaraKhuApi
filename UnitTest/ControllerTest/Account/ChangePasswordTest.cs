using System.Net;
using System.Threading.Tasks;
using Application.Features.Account.Commands.ChangePassword;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Account
{
    public class ChangePasswordTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Account/ChangePassword";

        public ChangePasswordTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task ChangePassword_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new ChangePasswordCommand
            {
                NewPassword = "NewPassword",
                OldPassword = "NotStudentPassword"
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
        
            //Output
            _outputHelper.WriteLine(await response.GetContent());
        
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode());
        }
    }
}