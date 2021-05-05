using System.Net;
using System.Threading.Tasks;
using Application.Features.Account.SignUp;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Account
{
    public class SignUpTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Account/SignUp";

        public SignUpTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task SignUp_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();

            var data = new SignUpCommand
            {
                FirstName = "Mohammad",
                LastName = "SharifiSadeghi",
                Email = "m.sharifisadeghi@khu.ac.ir",
                UserType = UserType.Student,
                Id= "982023016",
                Password="mohammad"
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
        public async Task SignUp_ShouldNotWork_DuplicateEmail()
        {
            var client = Host.GetTestClient();

            var data = new SignUpCommand
            {
                Email = "TestUser@Test.COM",
                Password = "TestPassword",
                FirstName = "Test",
                LastName = "Test",
                
            };

            //Act
            var response = await client.PostAsync(_path, data);

            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.DuplicateUser));
        }

        [Fact]
        public async Task SignUp_ShouldNotWork_InvalidInput()
        {
            // Arrange
            // Create an HttpClient to send requests to the TestServer
            var client = Host.GetTestClient();

            // Without Email
            var data = new SignUpCommand
            {
                Password = "TestPassword",
                FirstName = "Test",
                LastName = "Test",
            };

            //Act
            var response = await client.PostAsync(_path, data);

            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.InvalidInput));

            // Without Password
            data = new SignUpCommand
            {
                Email = "Test@InvalidInput.COM",
                FirstName = "Test",
                LastName = "Test",
                Id = "123",
                UserType = UserType.Instructor
            };

            //Act
            response = await client.PostAsync(_path, data);

            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.InvalidInput));

            // Without FirstName
             data = new SignUpCommand
            {
                Email = "Test@InvalidInput.COM",
                Password = "TestPassword",
                LastName = "Test",
            };

            //Act
            response = await client.PostAsync(_path, data);

            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.InvalidInput));

            // Without LastName
            data = new SignUpCommand
            {
                Email = "Test@InvalidInput.COM",
                Password = "TestPassword",
                FirstName = "Test",
            };

            //Act
            response = await client.PostAsync(_path, data);

            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.InvalidInput));

            // Password less than 8 characters
            data = new SignUpCommand
            {
                Email = "Test@InvalidInput.COM",
                Password = "TestPas",
                FirstName = "Test",
                LastName = "Test",
            };

            //Act
            response = await client.PostAsync(_path, data);

            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.InvalidInput));

            // Invalid Email
            data = new SignUpCommand
            {
                Email = "Test@.COM",
                Password = "TestPassword",
                FirstName = "Test",
                LastName = "Test",
            };

            //Act
            response = await client.PostAsync(_path, data);

            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.InvalidInput));
        }
    }
}
