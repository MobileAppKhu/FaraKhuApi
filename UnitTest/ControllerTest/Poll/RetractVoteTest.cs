using System.Net;
using System.Threading.Tasks;
using Application.Features.Poll.Commands.RetractVote;
using Application.Features.Poll.Commands.Vote;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Poll
{
    public class RetractVoteTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Poll/RetractVote";

        public RetractVoteTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task RetractVote_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToSecondStudent();

            var data = new RetractVoteCommand()
            {
                AnswerId = "Answer7"
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
        public async Task RetractVote_CantRetractVoteTheClosePoll()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToSecondStudent();

            var data = new RetractVoteCommand()
            {
                AnswerId = "Answer5"
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