using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Features.User.Queries.SearchUser;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.User
{
    public class SearchUserTest : AppFactory
    {
        private readonly string _path = "/api/User/SearchUser";
        private readonly ITestOutputHelper _outputHelper;

        public SearchUserTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task SearchUser_SearchingByFirstname()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SearchUserQuery
            {
                FirstName = "Owner",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchUserViewModel searchResult = (SearchUserViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchUserViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Users.Count == 1);
            Assert.True(searchResult.Users[0].UserId == "OwnerId");
        }

        [Fact]
        public async Task SearchUser_SearchingByLastname()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SearchUserQuery
            {
                LastName = "Officer",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchUserViewModel searchResult = (SearchUserViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchUserViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Users.Count == 1);
            Assert.True(searchResult.Users[0].UserId == "OfficerId");
        }

        [Fact]
        public async Task SearchUser_SearchingByGoogleScholar()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SearchUserQuery
            {
                GoogleScholar = "TestGoogleScholar",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchUserViewModel searchResult = (SearchUserViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchUserViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Users.Count == 1);
            Assert.True(searchResult.Users[0].UserId == "SearchStudentId");
        }

        [Fact]
        public async Task SearchUser_SearchingByLinkedIn()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SearchUserQuery
            {
                LinkedIn = "TestLinkedIn",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchUserViewModel searchResult = (SearchUserViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchUserViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Users.Count == 1);
            Assert.True(searchResult.Users[0].UserId == "SearchStudentId");
        }
        
        [Fact]
        public async Task SearchUser_OrderingByUserId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchUserQuery()
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                UserColumn = UserColumn.Id
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchUserViewModel searchResult = (SearchUserViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchUserViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Users.SequenceEqual(searchResult.Users.OrderBy(c => c.UserId).ToList()));
        }
        
        [Fact]
        public async Task SearchUser_OrderingByUserFirstname()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchUserQuery()
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                UserColumn = UserColumn.Firstname
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchUserViewModel searchResult = (SearchUserViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchUserViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Users.SequenceEqual(searchResult.Users.OrderBy(c => c.FirstName).ToList()));
        }
        
        [Fact]
        public async Task SearchUser_OrderingByUserLastname()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchUserQuery()
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                UserColumn = UserColumn.Lastname
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchUserViewModel searchResult = (SearchUserViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchUserViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Users.SequenceEqual(searchResult.Users.OrderBy(c => c.LastName).ToList()));
        }
        
        [Fact]
        public async Task SearchUser_OrderingByUserGoogleScholar()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchUserQuery()
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                UserColumn = UserColumn.GoogleScholar
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchUserViewModel searchResult = (SearchUserViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchUserViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Users.SequenceEqual(searchResult.Users.OrderBy(c => c.GoogleScholar).ToList()));
        }
        
        [Fact]
        public async Task SearchUser_OrderingByUserLinkedIn()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchUserQuery()
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                UserColumn = UserColumn.LinkedIn
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchUserViewModel searchResult = (SearchUserViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchUserViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Users.SequenceEqual(searchResult.Users.OrderBy(c => c.LinkedIn).ToList()));
        }
    }
}