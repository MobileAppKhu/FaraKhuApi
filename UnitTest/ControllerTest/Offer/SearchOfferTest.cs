using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Features.Offer.Queries.SearchOffers;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Offer
{
    public class SearchOfferTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Offer/SearchOffers";

        public SearchOfferTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task SearchOffer_SearchingByOfferType()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchOffersQuery()
            {
                OfferType = OfferType.Sell,
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchOffersViewModel searchResult = (SearchOffersViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchOffersViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Offer.Count == 1);
            Assert.True(searchResult.Offer[0].OfferId == "SearchOfferId");
        }
        
        [Fact]
        public async Task SearchOffer_SearchingByTitle()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchOffersQuery()
            {
                Search = "SearchTitle",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchOffersViewModel searchResult = (SearchOffersViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchOffersViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Offer.Count == 1);
            Assert.True(searchResult.Offer[0].OfferId == "SearchOfferId");
        }
        
        [Fact]
        public async Task SearchOffer_SearchingByPrice()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchOffersQuery()
            {
                StartPrice = "9000",
                EndPrice = "10000",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchOffersViewModel searchResult = (SearchOffersViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchOffersViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Offer.Count == 1);
            Assert.True(searchResult.Offer[0].OfferId == "SearchOfferId");
        }
        
        [Fact]
        public async Task SearchOffer_SearchingByUser()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchOffersQuery()
            {
                User = "SecondInstructorId",
                Start = 0,
                Step = 25
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchOffersViewModel searchResult = (SearchOffersViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchOffersViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Offer.Count == 1);
           // Assert.True(searchResult.Offer[0].OfferId == "SearchOfferId");
        }
        
        [Fact]
        public async Task SearchOffer_OrderingByOfferId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchOffersQuery()
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                OfferColumn = OfferColumn.OfferId
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchOffersViewModel searchResult = (SearchOffersViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchOffersViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Offer.SequenceEqual(searchResult.Offer.OrderBy(c => c.OfferId).ToList()));
        }
        
        [Fact]
        public async Task SearchOffer_OrderingByTitle()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchOffersQuery()
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                OfferColumn = OfferColumn.Title
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchOffersViewModel searchResult = (SearchOffersViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchOffersViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Offer.SequenceEqual(searchResult.Offer.OrderBy(c => c.Title).ToList()));
        }
        
        [Fact]
        public async Task SearchOffer_OrderingByDescription()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchOffersQuery()
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                OfferColumn = OfferColumn.Description
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchOffersViewModel searchResult = (SearchOffersViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchOffersViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Offer.SequenceEqual(searchResult.Offer.OrderBy(c => c.Description).ToList()));
        }
        
        [Fact]
        public async Task SearchOffer_OrderingByOfferType()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchOffersQuery()
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                OfferColumn = OfferColumn.OfferType
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchOffersViewModel searchResult = (SearchOffersViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchOffersViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Offer.SequenceEqual(searchResult.Offer.OrderBy(c => c.OfferType).ToList()));
        }
        
        [Fact]
        public async Task SearchOffer_OrderingByPrice()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchOffersQuery()
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                OfferColumn = OfferColumn.Price
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchOffersViewModel searchResult = (SearchOffersViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchOffersViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Offer.SequenceEqual(searchResult.Offer.OrderBy(c => c.Price).ToList()));
        }
        
        [Fact]
        public async Task SearchOffer_OrderingByCreationDate()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchOffersQuery()
            {
                Start = 0,
                Step = 25,
                OrderDirection = true,
                OfferColumn = OfferColumn.CreationDate
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            SearchOffersViewModel searchResult = (SearchOffersViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchOffersViewModel));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(searchResult.Offer.SequenceEqual(searchResult.Offer.OrderBy(c => c.CreatedDate).ToList()));
        }
    }
}