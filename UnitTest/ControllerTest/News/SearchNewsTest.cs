using System.Threading.Tasks;
using Application.Features.News.Queries.SearchNews;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.News
{
    public class SearchNewsTest : AppFactory
    {
        private readonly string _path = "/api/News/SearchNews";
        private readonly ITestOutputHelper _outputHelper;

        public SearchNewsTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task SearchNews_SearchingById()
        {
            //Arrange
            var client = Host.GetTestClient();

            var data = new SearchNewsQuery
            {

            };
        }

    }
}