using System.Net;
using System.Threading.Tasks;
using Application.Features.User.Queries.SearchAllEvents;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.User
{
    public class GetAllEventsTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "/api/User/GetAllEvents";

        public GetAllEventsTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task GetAllEvents_ShouldWorkCorrectlyForStudent()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SearchAllEventsQuery();
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        }
        
        [Fact]
        public async Task GetAllEvents_ShouldWorkCorrectlyForInstructor()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new SearchAllEventsQuery();
            
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