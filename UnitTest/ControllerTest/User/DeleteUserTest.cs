using System.Net;
using System.Threading.Tasks;
using Application.Features.User.Commands.DeleteUser;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.User
{
    public class DeleteUserTest : AppFactory
    {
        private readonly string _path = "/api/User/DeleteUser";
        private readonly ITestOutputHelper _outputHelper;

        public DeleteUserTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task DeleteUser_DeleteStudentShouldWorkCorrectly()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new DeleteUserCommand
            {
                UserId = "DeleteStudentId"
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
        public async Task DeleteUser_DeletePROfficerShouldWorkCorrectly()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new DeleteUserCommand
            {
                UserId = "DeletePROfficerId"
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
        public async Task DeleteUser_DeleteInstructorShouldWorkCorrectly()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new DeleteUserCommand
            {
                UserId = "DeleteInstructorId"
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
        public async Task DeleteUser_UserShouldNotBeFound()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new DeleteUserCommand
            {
                UserId = "WrongUserId"
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