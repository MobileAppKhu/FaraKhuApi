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
                UserType= UserType.Student,
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
                FirstName = "Mehrad",
                LastName = "Moshiri",
                Email = "mehradmoshiri@khu.ac.ir",
                UserType = UserType.Student,
                Id = "982023025",
                Password = "mehrad1379"
            };

            //Act
            var response = await client.PostAsync(_path, data);

            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode());
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
                FirstName = "Mohammad",
                LastName = "SharifiSadeghi",
                UserType = UserType.Student,
                Id= "982023016",
                Password="mohammad"
            };

            //Act
            var response = await client.PostAsync(_path, data);

            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            //Assert.True(await response.HasErrorCode(ErrorType.InvalidInput));

            // Without Password
            data = new SignUpCommand
            {
                FirstName = "Mohammad",
                LastName = "SharifiSadeghi",
                Email = "m.sharifisadeghi@khu.ac.ir",
                UserType = UserType.Student,
                Id= "982023016",
            };

            //Act
            response = await client.PostAsync(_path, data);

            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            //Assert.True(await response.HasErrorCode(ErrorType.InvalidInput));

            // Without FirstName
             data = new SignUpCommand
            {
                LastName = "SharifiSadeghi",
                Email = "m.sharifisadeghi@khu.ac.ir",
                UserType = UserType.Student,
                Id= "982023016",
                Password="mohammad"
            };

            //Act
            response = await client.PostAsync(_path, data);

            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            //Assert.True(await response.HasErrorCode(ErrorType.InvalidInput));

            // Without LastName
            data = new SignUpCommand
            {
                FirstName = "Mohammad",
                Email = "m.sharifisadeghi@khu.ac.ir",
                UserType = UserType.Student,
                Id= "982023016",
                Password="mohammad"
            };

            //Act
            response = await client.PostAsync(_path, data);

            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            //Assert.True(await response.HasErrorCode(ErrorType.InvalidInput));

            // Password less than 8 characters
            data = new SignUpCommand
            {
                FirstName = "Mohammad",
                LastName = "SharifiSadeghi",
                Email = "m.sharifisadeghi@khu.ac.ir",
                UserType = UserType.Student,
                Id= "982023016",
                Password="moham"
            };

            //Act
            response = await client.PostAsync(_path, data);

            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            //Assert.True(await response.HasErrorCode(ErrorType.InvalidInput));

            // Invalid Email
            data = new SignUpCommand
            {
                FirstName = "Mohammad",
                LastName = "SharifiSadeghi",
                Email = "m.sharifisadeghikhu.ac.ir",
                UserType = UserType.Student,
                Id= "982023016",
                Password="mohammad"
            };

            //Act
            response = await client.PostAsync(_path, data);

            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            //Assert.True(await response.HasErrorCode(ErrorType.InvalidInput));
        }
    }
}
