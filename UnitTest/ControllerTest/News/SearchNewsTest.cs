using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Features.News.Queries.SearchNews;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
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
        public async Task SearchNews_SearchingByTitleAndDescription()
        {
            //Arrange
            var client = Host.GetTestClient();

            var data = new SearchNewsQuery
            {
                Search = "search",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchNewsViewModel searchResult = (SearchNewsViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchNewsViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.News.Count == 1);
            Assert.True(searchResult.News[0].NewsId == "SearchNewsId");
        }

        [Fact]
        public async Task SearchNews_OrderingByNewsId()
        {
            //Arrange
            var client = Host.GetTestClient();

            var data = new SearchNewsQuery
            {
                Start = 0,
                Step = 25,
                NewsColumn = NewsColumn.NewsId,
                OrderDirection = true
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchNewsViewModel searchResult = (SearchNewsViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchNewsViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.News.SequenceEqual(searchResult.News.OrderBy(n => n.NewsId).ToList()));
        }

        [Fact]
        public async Task SearchNews_OrderingByTitle()
        {
            //Arrange
            var client = Host.GetTestClient();

            var data = new SearchNewsQuery
            {
                Start = 0,
                Step = 25,
                NewsColumn = NewsColumn.Title,
                OrderDirection = true
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchNewsViewModel searchResult = (SearchNewsViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchNewsViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.News.SequenceEqual(searchResult.News.OrderBy(n => n.Title).ToList()));
        }

        [Fact]
        public async Task SearchNews_OrderingByDescription()
        {
            //Arrange
            var client = Host.GetTestClient();

            var data = new SearchNewsQuery
            {
                Start = 0,
                Step = 25,
                NewsColumn = NewsColumn.Description,
                OrderDirection = true
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchNewsViewModel searchResult = (SearchNewsViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchNewsViewModel));
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.News.SequenceEqual(searchResult.News.OrderBy(n => n.Description).ThenBy(n => n.NewsId).ToList()));
        }

        [Fact]
        public async Task SearchNews_OrderingByCreationDate()
        {
            //Arrange
            var client = Host.GetTestClient();

            var data = new SearchNewsQuery
            {
                Start = 0,
                Step = 25,
                NewsColumn = NewsColumn.CreationDate,
                OrderDirection = true
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchNewsViewModel searchResult = (SearchNewsViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchNewsViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.News.SequenceEqual(searchResult.News.OrderBy(n => n.CreatedDate).ToList()));
        }
    }
}