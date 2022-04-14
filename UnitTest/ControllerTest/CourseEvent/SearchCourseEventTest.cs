using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Features.Course.Queries.SearchCourse;
using Application.Features.CourseEvent.Queries.SearchCourseEvent;
using Domain.Models;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.CourseEvent
{
    public class SearchCourseEventTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/CourseEvent/SearchCourseEvent";

        public SearchCourseEventTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task SearchCourseEvent_SearchingByCourseEventId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseEventQuery
            {
                CourseEventId = "3",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseEventViewModel searchResult = (SearchCourseEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.CourseEvents.Count == 1);
            Assert.True(searchResult.CourseEvents[0].CourseEventId == "3");
        }
        
        [Fact]
        public async Task SearchCourseEvent_SearchingByEventName()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseEventQuery
            {
                EventName = "SearchEventName",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseEventViewModel searchResult = (SearchCourseEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.CourseEvents.Count == 1);
            Assert.True(searchResult.CourseEvents[0].EventName == "SearchEventName");
        }
        
        [Fact]
        public async Task SearchCourseEvent_SearchingByEventDescription()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseEventQuery
            {
                EventDescription = "SearchDescription",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseEventViewModel searchResult = (SearchCourseEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.CourseEvents.Count == 1);
            Assert.True(searchResult.CourseEvents[0].Description == "SearchDescription");
        }
        
        [Fact]
        public async Task SearchCourseEvent_SearchingByCourseId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseEventQuery
            {
                CourseId = "SearchCourseId",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseEventViewModel searchResult = (SearchCourseEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.CourseEvents.Count == 1);
            Assert.True(searchResult.CourseEvents[0].CourseId == "SearchCourseId");
        }
        
        [Fact]
        public async Task SearchCourseEvent_OrderingByCourseId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseEventQuery
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                CourseEventColumn = CourseEventColumn.CourseId
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseEventViewModel searchResult = (SearchCourseEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.CourseEvents.SequenceEqual(searchResult.CourseEvents.OrderBy(c => c.CourseId).ToList()));
        }
        
        [Fact]
        public async Task SearchCourseEvent_OrderingByEventDescription()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseEventQuery
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                CourseEventColumn = CourseEventColumn.CourseEventDescription
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseEventViewModel searchResult = (SearchCourseEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.CourseEvents.SequenceEqual(searchResult.CourseEvents.OrderBy(c => c.Description).ToList()));
        }
        
        [Fact]
        public async Task SearchCourseEvent_OrderingByEventName()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseEventQuery
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                CourseEventColumn = CourseEventColumn.CourseEventName
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseEventViewModel searchResult = (SearchCourseEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.CourseEvents.SequenceEqual(searchResult.CourseEvents.OrderBy(c => c.EventName).ToList()));
        }
        
        [Fact]
        public async Task SearchCourseEvent_OrderingByCourseEventId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseEventQuery
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                CourseEventColumn = CourseEventColumn.CourseEventId
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseEventViewModel searchResult = (SearchCourseEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseEventViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.CourseEvents.SequenceEqual(searchResult.CourseEvents.OrderBy(c => c.CourseEventId).ToList()));
        }
    }
}