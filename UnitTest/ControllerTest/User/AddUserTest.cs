using System.Net;
using System.Threading.Tasks;
using Application.Features.User.Commands.AddUser;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.User
{
    public class AddUserTest : AppFactory
    {
        private readonly string _path = "/api/User/AddUser";
        private readonly ITestOutputHelper _outputHelper;

        public AddUserTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task AddUser_AddPROfficerShouldWorkCorrectly()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new AddUserCommand
            {
                Email = "profficer@test.test",
                Id = "",
                Password = "TestUserPassword",
                FirstName = "Test",
                LastName = "Test",
                GoogleScholar = "",
                LinkedIn = "",
                UserType = UserType.PROfficer
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
        public async Task AddUser_AddStudentShouldWorkCorrectly()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new AddUserCommand
            {
                Email = "student@test.test",
                Id = "",
                Password = "TestUserPassword",
                FirstName = "Test",
                LastName = "Test",
                GoogleScholar = "",
                LinkedIn = "",
                UserType = UserType.Student
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
        public async Task AddUser_AddInstructorShouldWorkCorrectly()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new AddUserCommand
            {
                Email = "instructor@test.test",
                Id = "",
                Password = "TestUserPassword",
                FirstName = "Test",
                LastName = "Test",
                GoogleScholar = "",
                LinkedIn = "",
                UserType = UserType.Instructor
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
        public async Task AddUser_InstructorIdShouldNotBeDuplicated()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new AddUserCommand
            {
                Email = "instructor1@test.test",
                Id = "12345",
                Password = "TestUserPassword",
                FirstName = "Test",
                LastName = "Test",
                GoogleScholar = "",
                LinkedIn = "",
                UserType = UserType.Instructor
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
        public async Task AddUser_StudentIdShouldNotBeDuplicated()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new AddUserCommand
            {
                Email = "student1@test.test",
                Id = "12345",
                Password = "TestUserPassword",
                FirstName = "Test",
                LastName = "Test",
                GoogleScholar = "",
                LinkedIn = "",
                UserType = UserType.Student
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
        public async Task AddUser_EmailShouldNotBeDuplicated()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new AddUserCommand
            {
                Email = "Student@Farakhu.app",
                Id = "12345",
                Password = "TestUserPassword",
                FirstName = "Test",
                LastName = "Test",
                GoogleScholar = "",
                LinkedIn = "",
                UserType = UserType.Student
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.DuplicateUser));
        }
    }
}