using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Application.Features.User.Commands.EditUser;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.User
{
    public class EditUserTest : AppFactory
    {
        private readonly string _path = "/api/User/EditUser";
        private readonly ITestOutputHelper _outputHelper;

        public EditUserTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task EditUser_EditUserFirstName()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new EditUserCommand
            {
                UserId = "EditUserId",
                FirstName = "NewFirstName"
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
        public async Task EditUser_EditUserLastName()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new EditUserCommand
            {
                UserId = "EditUserId",
                LastName = "NewLastName"
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
        public async Task EditUser_EditUserAvatar()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new EditUserCommand
            {
                UserId = "EditUserId",
                AvatarId = "sad.png"
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
        public async Task EditUser_DeleteUserAvatar()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new EditUserCommand
            {
                UserId = "EditUserId",
                DeleteAvatar = true
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
        public async Task EditUser_AddUserFavourites()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new EditUserCommand
            {
                UserId = "EditUserId",
                AddFavourites = new List<string>
                {
                    "Favourite"
                }
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
        public async Task EditUser_DeleteUserFavourites()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new EditUserCommand
            {
                UserId = "EditUserId",
                DeleteFavourites = new List<string>
                {
                    "EditUserFavouriteId"
                }
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
        public async Task EditUser_EditUserLinkedIn()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new EditUserCommand
            {
                UserId = "EditUserId",
                LinkedIn = "NewLinkedIn"
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
        public async Task EditUser_EditUserGoogleScholar()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new EditUserCommand
            {
                UserId = "EditUserId",
                GoogleScholar = "GoogleScholar"
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
        public async Task EditUser_UserShouldNotBeFound()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new EditUserCommand
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