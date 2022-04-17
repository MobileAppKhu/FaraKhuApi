using System.Net;
using System.Threading.Tasks;
using Application.Features.Poll.Queries.SearchAvailablePolls;
using Application.Features.Poll.Queries.SearchPoll;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Poll
{
    public class SearchAvailablePollTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Poll/SearchAvailablePolls";

        public SearchAvailablePollTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task SearchPoll_SearchingByPollCourseId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();
            var data = new SearchPollsQuery()
            {
                CourseId = "SearchCourseId",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchPollsViewModel searchResult = (SearchPollsViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchPollsViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Polls.Count == 1);
            Assert.True(searchResult.Polls[0].QuestionId == "SearchQuestionId");
        }
    }
}