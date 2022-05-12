using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Features.CourseType.Queries.SearchCourseType;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.CourseType
{
    public class SearchCourseTypeTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/CourseType/SearchCourseType";

        public SearchCourseTypeTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task SearchCourseType_SearchingById()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseTypeQuery()
            {
                CourseTypeId = "1",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseTypeViewModel searchResult = (SearchCourseTypeViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseTypeViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.CourseTypes.Count == 1);
            Assert.True(searchResult.CourseTypes[0].CourseTypeId == "1");
        }

        [Fact]
        public async Task SearchCourseType_SearchingByCode()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseTypeQuery()
            {
                CourseTypeCode = "11",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseTypeViewModel searchResult = (SearchCourseTypeViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseTypeViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.CourseTypes.Count == 1);
            Assert.True(searchResult.CourseTypes[0].CourseTypeId == "1");
        }

        [Fact]
        public async Task SearchCourseType_SearchingByTitle()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseTypeQuery()
            {
                CourseTypeTitle = "مبانی کامپیوتر",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseTypeViewModel searchResult = (SearchCourseTypeViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseTypeViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.CourseTypes.Count == 1);
            Assert.True(searchResult.CourseTypes[0].CourseTypeId == "1");
        }

        [Fact]
        public async Task SearchCourseType_SearchingByDepartmentId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseTypeQuery()
            {
                DepartmentId = "FirstDepartmentId",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseTypeViewModel searchResult = (SearchCourseTypeViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseTypeViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.CourseTypes.Count == 1);
        }

        [Fact]
        public async Task SearchCourseType_OrderingByCourseTypeId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseTypeQuery()
            {
                Start = 0,
                Step = 25,
                CourseTypeColumn = CourseTypeColumn.CourseTypeId,
                OrderDirection = true
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseTypeViewModel searchResult = (SearchCourseTypeViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseTypeViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.CourseTypes.SequenceEqual(searchResult.CourseTypes.OrderBy(c => c.CourseTypeId).ToList()));
        }

        [Fact]
        public async Task SearchCourseType_OrderingByCourseTypeCode()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseTypeQuery()
            {
                Start = 0,
                Step = 25,
                CourseTypeColumn = CourseTypeColumn.CourseTypeCode,
                OrderDirection = true
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseTypeViewModel searchResult = (SearchCourseTypeViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseTypeViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.CourseTypes.SequenceEqual(searchResult.CourseTypes.OrderBy(c => c.CourseTypeCode).ToList()));
        }

        [Fact]
        public async Task SearchCourseType_OrderingByCourseTypeTitle()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseTypeQuery()
            {
                Start = 0,
                Step = 25,
                CourseTypeColumn = CourseTypeColumn.CourseTypeTitle,
                OrderDirection = true
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseTypeViewModel searchResult = (SearchCourseTypeViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseTypeViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.CourseTypes.SequenceEqual(searchResult.CourseTypes.OrderBy(c => c.CourseTypeTitle).ToList()));
        }

        [Fact]
        public async Task SearchCourseType_OrderingByDepartmentId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseTypeQuery()
            {
                Start = 0,
                Step = 25,
                CourseTypeColumn = CourseTypeColumn.DepartmentId,
                OrderDirection = true
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseTypeViewModel searchResult = (SearchCourseTypeViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseTypeViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.CourseTypes.SequenceEqual(searchResult.CourseTypes.OrderBy(c => c.DepartmentId).ToList()));
        }
    }
}