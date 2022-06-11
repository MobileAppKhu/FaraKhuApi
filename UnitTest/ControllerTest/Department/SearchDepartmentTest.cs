using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Features.Department.Queries.SearchDepartment;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Department
{
    public class SearchDepartmentTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Department/SearchDepartment";

        public SearchDepartmentTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task SearchDepartment_SearchingById()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchDepartmentQuery()
            {
                DepartmentId = "FirstDepartmentId",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchDepartmentViewModel searchResult = (SearchDepartmentViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchDepartmentViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Departments.Count == 1);
            Assert.True(searchResult.Departments[0].DepartmentId == "FirstDepartmentId");
        }

        [Fact]
        public async Task SearchDepartment_SearchingByCode()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchDepartmentQuery()
            {
                DepartmentCode = "11",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchDepartmentViewModel searchResult = (SearchDepartmentViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchDepartmentViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Departments.Count == 1);
            Assert.True(searchResult.Departments[0].DepartmentId == "FirstDepartmentId");
        }

        [Fact]
        public async Task SearchDepartment_SearchingByTitle()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchDepartmentQuery()
            {
                DepartmentTitle = "کامپیوتر",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchDepartmentViewModel searchResult = (SearchDepartmentViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchDepartmentViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Departments.Count == 1);
            Assert.True(searchResult.Departments[0].DepartmentId == "FirstDepartmentId");
        }

        [Fact]
        public async Task SearchDepartment_SearchingByFacultyId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchDepartmentQuery()
            {
                FacultyId = "FirstFacultyId",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchDepartmentViewModel searchResult = (SearchDepartmentViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchDepartmentViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Departments.Count == 2);
        }

        [Fact]
        public async Task SearchDepartment_OrderingByDepartmentId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchDepartmentQuery()
            {
                Start = 0,
                Step = 25,
                DepartmentColumn = DepartmentColumn.DepartmentId,
                OrderDirection = true
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchDepartmentViewModel searchResult = (SearchDepartmentViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchDepartmentViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Departments.SequenceEqual(searchResult.Departments.OrderBy(c => c.DepartmentId).ToList()));
        }

        [Fact]
        public async Task SearchDepartment_OrderingByDepartmentCode()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchDepartmentQuery()
            {
                Start = 0,
                Step = 25,
                DepartmentColumn = DepartmentColumn.DepartmentCode,
                OrderDirection = true
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchDepartmentViewModel searchResult = (SearchDepartmentViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchDepartmentViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Departments.SequenceEqual(searchResult.Departments.OrderBy(c => c.DepartmentCode).ToList()));
        }

        [Fact]
        public async Task SearchDepartment_OrderingByDepartmentTitle()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchDepartmentQuery()
            {
                Start = 0,
                Step = 25,
                DepartmentColumn = DepartmentColumn.DepartmentTitle,
                OrderDirection = true
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchDepartmentViewModel searchResult = (SearchDepartmentViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchDepartmentViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Departments.SequenceEqual(searchResult.Departments.OrderBy(c => c.DepartmentTitle).ToList()));
        }

        [Fact]
        public async Task SearchDepartment_OrderingByFacultyId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchDepartmentQuery()
            {
                Start = 0,
                Step = 25,
                DepartmentColumn = DepartmentColumn.FacultyId,
                OrderDirection = true
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchDepartmentViewModel searchResult = (SearchDepartmentViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchDepartmentViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Departments.SequenceEqual(searchResult.Departments.OrderBy(c => c.FacultyId).ToList()));
        }
    }
}