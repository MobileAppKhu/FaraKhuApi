using System.Net;
using System.Threading.Tasks;
using Application.Features.Offer.Queries.SearchUserOffers;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Offer
{
    public class SearchUserOfferTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Offer/SearchUserOffers";

        public SearchUserOfferTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        
        [Fact]
        public async Task SearchUserOffers_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();
            var data = new SearchUserOffersQuery();
            
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}