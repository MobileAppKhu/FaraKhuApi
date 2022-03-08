using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Application.Features.Account.Commands.EditProfile;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Account
{
    public class EditProfileTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Account/EditProfile";

        public EditProfileTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task EditProfile_FirstName_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();
            
            var data = new EditProfileCommand
            {
                FirstName = "EditedStudentName",
                AddFavourites = new List<string>(),
                DeleteFavourites = new List<string>()
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
        public async Task EditProfile_LastName_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();
            
            var data = new EditProfileCommand
            {
                LastName = "EditedStudentName",
                AddFavourites = new List<string>(),
                DeleteFavourites = new List<string>()
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
        public async Task EditProfile_AddFavourite_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var AddFavourite = new List<string>();
            AddFavourite.Add("Java");
            var data = new EditProfileCommand
            {
                AddFavourites = AddFavourite,
                DeleteFavourites = new List<string>()
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
        public async Task EditProfile_LinkedIn_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new EditProfileCommand
            {
                AddFavourites = new List<string>(),
                DeleteFavourites = new List<string>(),
                LinkedIn = "LinkedIn"
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
        public async Task EditProfile_GoogleScholar_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new EditProfileCommand
            {
                AddFavourites = new List<string>(),
                DeleteFavourites = new List<string>(),
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
    }
}