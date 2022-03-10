using System;
using System.Net;
using System.Threading.Tasks;
using Application.Features.Event.Commands.AddEvent;
using Application.Features.Offer.Commands.AddOffer;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Offer
{
    public class AddOfferTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Offer/AddOffer";
        
        public AddOfferTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task AddOffer_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new AddOfferCommand()
            {
                Description = "description",
                Price = "1000",
                Title = "Title",
                AvatarId = "smiley.png",
                OfferType = OfferType.Sell
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        }
        
        [Fact]
        public async Task AddOffer_SholudNotCreatOfferWithOutAvatar()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new AddOfferCommand()
            {
                Description = "description",
                Price = "1000",
                Title = "Title",
                OfferType = OfferType.Sell
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode());
        }
        
        [Fact]
        public async Task AddOffer_SholudNotCreatOfferWithOutPrice()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new AddOfferCommand()
            {
                Description = "description",
                Title = "Title",
                AvatarId = "smiley.png",
                OfferType = OfferType.Sell
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode());
        }
        
        [Fact]
        public async Task AddOffer_SholudNotCreatOfferWithOutTitle()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new AddOfferCommand()
            {
                Description = "description",
                Price = "1000",
                AvatarId = "smiley.png",
                OfferType = OfferType.Sell
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode());
        }
        
        [Fact]
        public async Task AddOffer_SholudNotCreatOfferWithOutDescription()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new AddOfferCommand()
            {
                Title = "Title",
                Price = "1000",
                AvatarId = "smiley.png",
                OfferType = OfferType.Sell
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode());
        }
    }
}