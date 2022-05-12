using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Features.Event.Queries.SearchEvent;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Event
{
    public class SearchEventTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Event/SearchEvent";

        public SearchEventTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task SearchEvent_SearchingByEventId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchEventQuery
            {
                EventId = "SearchEventId",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchEventViewModel searchResult = (SearchEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Event.Count == 1);
            Assert.True(searchResult.Event[0].EventId == "SearchEventId");
        }
        
        [Fact]
        public async Task SearchEvent_SearchingByEventName()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchEventQuery
            {
                EventName = "SearchEventName",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchEventViewModel searchResult = (SearchEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Event.Count == 1);
            Assert.True(searchResult.Event[0].EventName == "SearchEventName");
        }
        
        [Fact]
        public async Task SearchEvent_SearchingByDescription()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchEventQuery
            {
                Description = "SearchEventDescription",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchEventViewModel searchResult = (SearchEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Event.Count == 1);
            Assert.True(searchResult.Event[0].EventDescription == "SearchEventDescription");
        }
        
        [Fact]
        public async Task SearchEvent_SearchingByEventTime()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchEventQuery
            {
                EventTime = DateTime.MaxValue,
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchEventViewModel searchResult = (SearchEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Event.Count == 1);
            Assert.True(searchResult.Event[0].EventId == "SearchEventId");
        }
        
        [Fact]
        public async Task SearchEvent_SearchingByCourseId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchEventQuery
            {
                CourseId = "SearchCourseId",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchEventViewModel searchResult = (SearchEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Event.Count == 1);
            Assert.True(searchResult.Event[0].CourseId == "SearchCourseId");
        }
        
        [Fact]
        public async Task SearchEvent_OrderingByEventId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchEventQuery
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                EventColumn = EventColumn.EventId
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchEventViewModel searchResult = (SearchEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Event.SequenceEqual(searchResult.Event.OrderBy(c => c.EventId).ToList()));
        }
        
        [Fact]
        public async Task SearchEvent_OrderingByEventName()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchEventQuery
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                EventColumn = EventColumn.EventName
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchEventViewModel searchResult = (SearchEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Event.SequenceEqual(searchResult.Event.OrderBy(c => c.EventName).ToList()));
        }
        
        [Fact]
        public async Task SearchEvent_OrderingByDescription()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchEventQuery
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                EventColumn = EventColumn.EventDescription
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchEventViewModel searchResult = (SearchEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Event.SequenceEqual(searchResult.Event.OrderBy(c => c.EventDescription).ToList()));
        }
        
        [Fact]
        public async Task SearchEvent_OrderingByEventTime()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchEventQuery
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                EventColumn = EventColumn.EventTime
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchEventViewModel searchResult = (SearchEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Event.SequenceEqual(searchResult.Event.OrderBy(c => c.EventTime).ToList()));
        }
        
        [Fact]
        public async Task SearchEvent_OrderingByCreationDate()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchEventQuery
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                EventColumn = EventColumn.CreationDate
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchEventViewModel searchResult = (SearchEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Event.SequenceEqual(searchResult.Event.OrderBy(c => c.CreatedDate).ToList()));
        }
        
    }
}