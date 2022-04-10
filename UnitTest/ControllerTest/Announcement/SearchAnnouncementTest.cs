using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Features.Announcement.Queries.SearchAnnouncements;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Announcement
{
    public class SearchAnnouncementTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Announcement/SearchAnnouncements";

        public SearchAnnouncementTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task SearchAnnouncement_SearchingById()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SearchAnnouncementsQuery
            {   
                AnnouncementId = "SearchAnnouncement",
                Start = 0,
                Step = 25
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            _outputHelper.WriteLine(await response.GetContent());
            var searchResult = (SearchAnnouncementsViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchAnnouncementsViewModel));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult?.Announcements.Count == 1);
            Assert.True(searchResult.Announcements[0].AnnouncementId == "SearchAnnouncement");
        }

        [Fact]
        public async Task SearchAnnouncement_SearchingByDescription()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SearchAnnouncementsQuery
            {   
                Description = "Description2",
                Start = 0,
                Step = 25
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            _outputHelper.WriteLine(await response.GetContent());
            var searchResult = (SearchAnnouncementsViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchAnnouncementsViewModel));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult?.Announcements.Count == 1);
            Assert.True(searchResult.Announcements[0].AnnouncementId == "SearchAnnouncement");
        }

        [Fact]
        public async Task SearchAnnouncement_SearchingByTitle()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SearchAnnouncementsQuery
            {   
                Title = "Title2",
                Start = 0,
                Step = 25
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            _outputHelper.WriteLine(await response.GetContent());
            var searchResult = (SearchAnnouncementsViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchAnnouncementsViewModel));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult?.Announcements.Count == 1);
            Assert.True(searchResult.Announcements[0].AnnouncementId == "SearchAnnouncement");
        }

        [Fact]
        public async Task SearchAnnouncement_SearchingByUser()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SearchAnnouncementsQuery
            {   
                User = "SecondStudentId",
                Start = 0,
                Step = 25
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            _outputHelper.WriteLine(await response.GetContent());
            var searchResult = (SearchAnnouncementsViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchAnnouncementsViewModel));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult?.Announcements.Count == 1);
            Assert.True(searchResult.Announcements[0].AnnouncementId == "SearchAnnouncement");
        }

        [Fact]
        public async Task SearchAnnouncement_OrderingByAnnouncementIds()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SearchAnnouncementsQuery
            {   
                Start = 0,
                Step = 25,
                AnnouncementColumn = AnnouncementColumn.AnnouncementId,
                OrderDirection = true
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            _outputHelper.WriteLine(await response.GetContent());
            var searchResult = (SearchAnnouncementsViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchAnnouncementsViewModel));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Announcements.SequenceEqual(searchResult.Announcements.OrderBy(c => c.AnnouncementId).ToList()));
        }

        [Fact]
        public async Task SearchAnnouncement_OrderingByDescription()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SearchAnnouncementsQuery
            {   
                Start = 0,
                Step = 25,
                AnnouncementColumn = AnnouncementColumn.Description,
                OrderDirection = true
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            _outputHelper.WriteLine(await response.GetContent());
            var searchResult = (SearchAnnouncementsViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchAnnouncementsViewModel));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Announcements.SequenceEqual(searchResult.Announcements.OrderBy(c => c.AnnouncementDescription).ToList()));
        }

        [Fact]
        public async Task SearchAnnouncement_OrderingByTitle()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SearchAnnouncementsQuery
            {   
                Start = 0,
                Step = 25,
                AnnouncementColumn = AnnouncementColumn.Title,
                OrderDirection = true
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            _outputHelper.WriteLine(await response.GetContent());
            var searchResult = (SearchAnnouncementsViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchAnnouncementsViewModel));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Announcements.SequenceEqual(searchResult.Announcements.OrderBy(c => c.AnnouncementTitle).ToList()));
        }

        [Fact]
        public async Task SearchAnnouncement_OrderingByCreationDate()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SearchAnnouncementsQuery
            {   
                Start = 0,
                Step = 25,
                AnnouncementColumn = AnnouncementColumn.CreationDate,
                OrderDirection = true
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            _outputHelper.WriteLine(await response.GetContent());
            var searchResult = (SearchAnnouncementsViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchAnnouncementsViewModel));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Announcements.SequenceEqual(searchResult.Announcements.OrderBy(c => c.CreatedDate).ToList()));
        }

        [Fact]
        public async Task SearchAnnouncement_OrderingByUserId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new SearchAnnouncementsQuery
            {   
                Start = 0,
                Step = 25,
                AnnouncementColumn = AnnouncementColumn.UserId,
                OrderDirection = true
            };
            
            // Act
            var response = await client.PostAsync(_path, data);
            _outputHelper.WriteLine(await response.GetContent());
            var searchResult = (SearchAnnouncementsViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchAnnouncementsViewModel));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Announcements.SequenceEqual(searchResult.Announcements.OrderBy(c => c.UserId).ToList()));
        }
    }
}