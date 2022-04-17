using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Features.Ticket.Queries.SearchTicket;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Ticket
{
    public class SearchTicketTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Ticket/SearchTicket";

        public SearchTicketTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task SearchTicket_SearchingByTicketId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchTicketQuery()
            {
                TicketId = "SearchTicketId",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
             SearchTicketQueryViewModel searchResult = (SearchTicketQueryViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchTicketQueryViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.TicketDtos.Count == 1);
            Assert.True(searchResult.TicketDtos[0].TicketId == "SearchTicketId");
        }
        
        [Fact]
        public async Task SearchTicket_SearchingByTicketStatus()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchTicketQuery()
            {
                TicketStatus = TicketStatus.InProgress,
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
             SearchTicketQueryViewModel searchResult = (SearchTicketQueryViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchTicketQueryViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.TicketDtos.Count == 1);
            Assert.True(searchResult.TicketDtos[0].TicketId == "SearchTicketId");
        }
        
        [Fact]
        public async Task SearchTicket_SearchingByTicketPriority()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchTicketQuery()
            {
                TicketPriority = TicketPriority.Urgent,
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
             SearchTicketQueryViewModel searchResult = (SearchTicketQueryViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchTicketQueryViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.TicketDtos.Count == 1);
            Assert.True(searchResult.TicketDtos[0].TicketId == "SearchTicketId");
        }
        
        [Fact]
        public async Task SearchTicket_SearchingByDescription()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchTicketQuery()
            {
                Description = "SearchDescription",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
             SearchTicketQueryViewModel searchResult = (SearchTicketQueryViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchTicketQueryViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.TicketDtos.Count == 1);
            Assert.True(searchResult.TicketDtos[0].TicketId == "SearchTicketId");
        }
        
        [Fact]
        public async Task SearchTicket_OrderingByDescription()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchTicketQuery()
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                TicketColumn = TicketColumn.Description
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
             SearchTicketQueryViewModel searchResult = (SearchTicketQueryViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchTicketQueryViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.TicketDtos.SequenceEqual(searchResult.TicketDtos.OrderBy(c => c.Description).ToList()));
        }
        
        [Fact]
        public async Task SearchTicket_OrderingByTicketId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchTicketQuery()
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                TicketColumn = TicketColumn.TicketId
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
             SearchTicketQueryViewModel searchResult = (SearchTicketQueryViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchTicketQueryViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.TicketDtos.SequenceEqual(searchResult.TicketDtos.OrderBy(c => c.TicketId).ToList()));
        }
        
        [Fact]
        public async Task SearchTicket_OrderingByPriority()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchTicketQuery()
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                TicketColumn = TicketColumn.Priority
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
             SearchTicketQueryViewModel searchResult = (SearchTicketQueryViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchTicketQueryViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.TicketDtos.SequenceEqual(searchResult.TicketDtos.OrderBy(c => c.Priority).ToList()));
        }
        
        [Fact]
        public async Task SearchTicket_OrderingByStatus()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchTicketQuery()
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                TicketColumn = TicketColumn.Status
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
             SearchTicketQueryViewModel searchResult = (SearchTicketQueryViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchTicketQueryViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.TicketDtos.SequenceEqual(searchResult.TicketDtos.OrderBy(c => c.Status).ToList()));
        }
        
        [Fact]
        public async Task SearchTicket_OrderingByDeadLine()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchTicketQuery()
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                TicketColumn = TicketColumn.DeadLine
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
             SearchTicketQueryViewModel searchResult = (SearchTicketQueryViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchTicketQueryViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.TicketDtos.SequenceEqual(searchResult.TicketDtos.OrderBy(c => c.DeadLine).ToList()));
        }
        
        [Fact]
        public async Task SearchTicket_OrderingByCreationDate()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchTicketQuery()
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                TicketColumn = TicketColumn.CreationDate
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
             SearchTicketQueryViewModel searchResult = (SearchTicketQueryViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchTicketQueryViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.TicketDtos.SequenceEqual(searchResult.TicketDtos.OrderBy(c => c.CreatedDate).ToList()));
        }
    }
}