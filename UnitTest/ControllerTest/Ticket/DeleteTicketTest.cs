using System.Net;
using System.Threading.Tasks;
using Application.Features.Ticket.Commands.DeleteTicket;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Ticket
{
    public class DeleteTicketTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Ticket/Deleteticket";
        
        public DeleteTicketTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task DeleteTicket_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new DeleteTicketCommand()
            {
                TicketId = "DeleteTicketId"
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
        public async Task DeleteTicket_TicketShouldNotBeFound()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new DeleteTicketCommand()
            {
                TicketId = "NotTicketId"
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
        public async Task DeleteTicket_AnotherUserShouldNotDelete()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToSecondInstructor();

            var data = new DeleteTicketCommand()
            {
                TicketId = "TicketId"
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.Unauthorized));
        }
        
        [Fact]
        public async Task DeleteTicket_AnotherUserTypeShouldNotDelete()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new DeleteTicketCommand()
            {
                TicketId = "TicketId"
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