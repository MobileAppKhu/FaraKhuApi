using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Features.Faculty.Queries;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Faculty
{
    public class SearchFacultyTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Faculty/SearchFaculty";

        public SearchFacultyTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task SearchFaculty_SearchingById()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchFacultyQuery()
            {
                FacultyId = "FirstFacultyId",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchFacultyViewModel searchResult = (SearchFacultyViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchFacultyViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Faculties.Count == 1);
            Assert.True(searchResult.Faculties[0].FacultyId == "FirstFacultyId");
        }
        
        [Fact]
        public async Task SearchFaculty_SearchingByFacultyCode()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchFacultyQuery()
            {
                FacultyCode = "1",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchFacultyViewModel searchResult = (SearchFacultyViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchFacultyViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Faculties.Count == 1);
            Assert.True(searchResult.Faculties[0].FacultyId == "FirstFacultyId");
        }
        
        [Fact]
        public async Task SearchFaculty_SearchingByFacultyTitle()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchFacultyQuery()
            {
                FacultyTitle = "فنی و مهندسی",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchFacultyViewModel searchResult = (SearchFacultyViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchFacultyViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Faculties.Count == 1);
            Assert.True(searchResult.Faculties[0].FacultyId == "FirstFacultyId");
        }
        
        [Fact]
        public async Task SearchFaculty_OrderingByFacultyId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchFacultyQuery()
            {
                Start = 0,
                Step = 25,
                FacultyColumn = FacultyColumn.FacultyId,
                OrderDirection = true
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchFacultyViewModel searchResult = (SearchFacultyViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchFacultyViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Faculties.SequenceEqual(searchResult.Faculties.OrderBy(c => c.FacultyId).ToList()));
        }
        
        [Fact]
        public async Task SearchFaculty_OrderingByFacultyCode()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchFacultyQuery()
            {
                Start = 0,
                Step = 25,
                FacultyColumn = FacultyColumn.FacultyCode,
                OrderDirection = true
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchFacultyViewModel searchResult = (SearchFacultyViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchFacultyViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Faculties.SequenceEqual(searchResult.Faculties.OrderBy(c => c.FacultyCode).ToList()));
        }
        
        [Fact]
        public async Task SearchFaculty_OrderingByFacultyTitle()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchFacultyQuery()
            {
                Start = 0,
                Step = 25,
                FacultyColumn = FacultyColumn.FacultyTitle,
                OrderDirection = true
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchFacultyViewModel searchResult = (SearchFacultyViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchFacultyViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Faculties.SequenceEqual(searchResult.Faculties.OrderBy(c => c.FacultyTitle).ToList()));
        }
    }
}