using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Features.Course.Queries.SearchCourse;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Course
{
    public class SearchCourseTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Course/SearchCourse";

        public SearchCourseTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task SearchCourse_SearchingById()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseQuery
            {
                CourseId = "CourseId",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseViewModel searchResult = (SearchCourseViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Course.Count == 1);
            Assert.True(searchResult.Course[0].CourseId == "CourseId");
        }
        
        [Fact]
        public async Task SearchCourse_SearchingByCourseTypeId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseQuery
            {
                CourseType = "1",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseViewModel searchResult = (SearchCourseViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Course.Count == 1);
            Assert.True(searchResult.Course[0].CourseId == "CourseId");
        }
        
        [Fact]
        public async Task SearchCourse_SearchingByInstructor()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseQuery
            {
                Instructor = "SecondInstructorId",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseViewModel searchResult = (SearchCourseViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Course.Count == 1);
            Assert.True(searchResult.Course[0].CourseId == "SearchCourseId");
        }
        
        [Fact]
        public async Task SearchCourse_SearchingByStudent()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseQuery
            {
                Student = "SearchStudentId",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseViewModel searchResult = (SearchCourseViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Course.Count == 1);
            Assert.True(searchResult.Course[0].CourseId == "SearchCourseId");
        }
        
        [Fact]
        public async Task SearchCourse_OrderingByCourseId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseQuery
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                CourseColumn = CourseColumn.CourseId
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseViewModel searchResult = (SearchCourseViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseViewModel));
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Assert.True(searchResult.Course.Equals(searchResult.Course.OrderBy(c => c.CourseId).ToList()));
        }
        
        [Fact]
        public async Task SearchCourse_OrderingByCreationDate()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseQuery
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                CourseColumn = CourseColumn.CreationDate
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseViewModel searchResult = (SearchCourseViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Assert.True(searchResult.Course.Equals(searchResult.Course.OrderBy(c => c.CreatedDate).ToList()));
        }
        
        [Fact]
        public async Task SearchCourse_OrderingByEndDate()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseQuery
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                CourseColumn = CourseColumn.EndDate
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseViewModel searchResult = (SearchCourseViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Assert.True(searchResult.Course.Equals(searchResult.Course.OrderBy(c => c.EndDate).ToList()));
        }
        
        [Fact]
        public async Task SearchCourse_OrderingByInstructorId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseQuery
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                CourseColumn = CourseColumn.InstructorId
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseViewModel searchResult = (SearchCourseViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Assert.True(searchResult.Course.Equals(searchResult.Course.OrderBy(c => c.InstructorId).ToList()));
        }
        [Fact]
        public async Task SearchCourse_OrderingByCourseTypeId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseQuery
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                CourseColumn = CourseColumn.CourseTypeId
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchCourseViewModel searchResult = (SearchCourseViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        public async Task SearchCourse_StudentShouldNotBeFound()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchCourseQuery
            {
                Start = 0,
                Step = 25,
                Student = "WrongStudentId"
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
        }
    }
}