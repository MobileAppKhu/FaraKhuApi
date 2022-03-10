using System.Net;
using System.Threading.Tasks;
using Application.Features.Offer.Commands.EditOffer;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Offer
{
    public class EditOfferTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Offer/EditOffer";
        
        public EditOfferTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task EditOfferTitle_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new EditOfferCommand()
            {
                OfferId = "OfferId",
                Title = "EditedTitle"
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
        public async Task EditOfferDescription_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new EditOfferCommand()
            {
                OfferId = "OfferId",
                Description = "EditedDescription"
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
        public async Task EditOfferOfferType_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new EditOfferCommand()
            {
                OfferId = "OfferId",
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
        public async Task EditOfferPrice_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new EditOfferCommand()
            {
                OfferId = "OfferId",
                Price = "2000"
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
        public async Task EditOfferAvatar_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new EditOfferCommand()
            {
                OfferId = "OfferId",
                AvatarId = "sad.png"
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
        public async Task EditOfferAvatar_ShouldNotWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new EditOfferCommand()
            {
                OfferId = "OfferId",
                AvatarId = "Not.png"
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
        public async Task EditOfferAvatar_AnotherUserShouldNotEdit()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToSecondInstructor();

            var data = new EditOfferCommand()
            {
                OfferId = "OfferId",
                Price = "2000"
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
        public async Task EditOfferAvatar_AnotherUserTypeShouldNotEdit()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new EditOfferCommand()
            {
                OfferId = "OfferId",
                Price = "2000"
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
        public async Task EditOfferAvatar_OfferShouldNotBeFound()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new EditOfferCommand()
            {
                OfferId = "NotOfferId",
                Price = "2000"
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