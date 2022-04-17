using System.Net;
using System.Threading.Tasks;
using Application.Features.Offer.Queries.SearchOffers;
using Application.Features.Poll.Queries.SearchPoll;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Poll
{
    public class SearchPollTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Poll/SearchPoll";

        public SearchPollTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task SearchPoll_SearchingByPollQuestionId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToOwner();
            var data = new SearchPollQuery()
            {
                QuestionId = "SearchQuestionId"
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchPollViewModel searchResult = (SearchPollViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchPollViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Question.QuestionId == "SearchQuestionId");
        }
    }
}