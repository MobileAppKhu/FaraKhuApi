using System.Net;
using System.Threading.Tasks;
using Application.Features.Notification.Queries.SearchNotification;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Notification
{
    public class SearchNotificationTest : AppFactory
    {
        private readonly string _path = "/api/Notification/SearchNotification";
        private readonly ITestOutputHelper _outputHelper;

        public SearchNotificationTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task SearchNotification_ShouldWorkCorrectly()
        {
            //Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SearchNotificationQuery();
            
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