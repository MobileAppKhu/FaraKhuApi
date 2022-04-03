using System.Net;
using System.Threading.Tasks;
using Application.Features.Poll.Commands.DeleteQuestion;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Poll
{
    public class DeleteQuestionTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Poll/DeletePollQuestion";
        
        public DeleteQuestionTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
    
        [Fact]
        public async Task DeleteQuestion_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new DeleteQuestionCommand()
            {
                QuestionId = "DeleteQuestionId"
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
        public async Task DeleteQuestion_QuestionShouldNotBeFound()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new DeleteQuestionCommand()
            {
                QuestionId = "NotQuestionId"
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
        public async Task DeleteQuestion_AnotherUserShouldNotDelete()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToSecondInstructor();

            var data = new DeleteQuestionCommand()
            {
                QuestionId = "DeleteQuestionId"
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
        public async Task DeleteQuestion_AnotherUserTypeShouldNotDelete()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new DeleteQuestionCommand()
            {
                QuestionId = "DeleteQuestionId"
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.True(!await response.HasErrorCode());
        }
    }
}