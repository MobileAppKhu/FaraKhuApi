using System.Net;
using System.Threading.Tasks;
using Application.Features.Event.Queries.GetIncomingEvent;
using Application.Features.Event.Queries.SearchEvent;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Event
{
    public class GetIncomingEventTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Event/GetIncomingEvents";

        public GetIncomingEventTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task GetIncomingEvent_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new GetIncomingEventQuery();
            
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}