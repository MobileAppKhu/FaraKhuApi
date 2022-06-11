using System.Net;
using System.Threading.Tasks;
using Application.Features.Account.Commands.SignIn;
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
        
        [Theory]
        [InlineData("Student@Farakhu.app", "StudentPassword")]
        [InlineData("SecondStudent@Farakhu.app", "SecondStudentPassword")]
        [InlineData("Instructor@Farakhu.app", "InstructorPassword")]
        [InlineData("PublicRelation@FaraKhu.app", "PROfficerPassword")]
        [InlineData("Owner@Farakhu.app", "OwnerPassword")]
        public async Task SignIn_ShouldWorkCorrectly(string logon, string password)
        {
            // Arrange
            var client = Host.GetTestClient();
        
            var data = new SignInCommand
            {
                Logon = logon,
                Password = password
            };
        
            //Act
            var response = await client.PostAsync(_path, data);
        
            //Output
            _outputHelper.WriteLine(await response.GetContent());
        
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        }
        
        [Theory]
        [InlineData("Student@Farakhu.app", "NotStudentPassword")]
        [InlineData("Instructor@Farakhu.com", "InstructorPassword")]
        public async Task SignIn_ShouldNotWork(string logon, string password)
        {
            // Arrange
            var client = Host.GetTestClient();
        
            var data = new SignInCommand
            {
                Logon = logon,
                Password = password
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