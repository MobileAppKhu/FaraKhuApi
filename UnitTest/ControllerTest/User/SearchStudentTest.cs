using System.Net;
using System.Threading.Tasks;
using Application.Features.User.Queries.SearchStudent;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.User
{
    public class SearchStudentTest : AppFactory
    {
        private readonly string _path = "/api/User/SearchStudent";
        private readonly ITestOutputHelper _outputHelper;

        public SearchStudentTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task SearchStudent_ShouldWorkCorrectly()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new SearchStudentQuery
            {
                StudentId = "12345"
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchStudentViewModel searchResult = (SearchStudentViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchStudentViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.StudentShortDto.Id == "StudentId");
        }

        [Fact]
        public async Task SearchStudent_StudentShouldNotBeFound()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();

            var data = new SearchStudentQuery
            {
                StudentId = "WrongStudentId"
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.StudentNotFound));
        }
    }
}