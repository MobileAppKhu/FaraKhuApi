using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Application.Features.Poll.Commands.AddQuestion;
using Domain.Enum;
using Microsoft.AspNetCore.TestHost;
using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.Poll
{
    public class AddQuestionTest : AppFactory
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/Poll/AddPollQuestion";
        
        public AddQuestionTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task AddQuestion_ShouldWorkCorrectly()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new AddQuestionCommand()
            {
                CourseId = "CourseId",
                MultiVote = "true",
                QuestionDescription = "Description",
                Answers = new List<string>()
                {
                   "Answer1",
                   "Answer2"
                }
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
        public async Task AddQuestion_ShouldNotCreatePollWithOutCourseId()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new AddQuestionCommand()
            {
                MultiVote = "true",
                QuestionDescription = "Description",
                Answers = new List<string>()
                {
                   "Answer1",
                   "Answer2"
                }
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.InvalidInput));
        }
        
        [Fact]
        public async Task AddQuestion_ShouldNotCreatePollWithOutAnswer()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new AddQuestionCommand()
            {
                MultiVote = "true",
                QuestionDescription = "Description",
                CourseId = "CourseId"
            };
            
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.InvalidInput));
        }
        
        [Fact]
        public async Task AddQuestion_ShouldNotCreatOfferWithOutDescription()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToInstructor();

            var data = new AddQuestionCommand()
            {
                MultiVote = "true",
                CourseId = "CourseId",
                Answers = new List<string>()
                {
                   "Answer1",
                   "Answer2"
                }
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.InvalidInput));
        }
        
        [Fact]
        public async Task AddQuestion_AntoherUserTypeShouldNotAdd()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToStudent();

            var data = new AddQuestionCommand()
            {
                MultiVote = "true",
                CourseId = "CourseId",
                QuestionDescription = "description",
                Answers = new List<string>()
                {
                   "Answer1",
                   "Answer2"
                }
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.True(!await response.HasErrorCode(ErrorType.Unauthorized));
        }
        
        [Fact]
        public async Task AddQuestion_AntoherUserShouldNotAdd()
        {
            // Arrange
            var client = Host.GetTestClient();
            await client.AuthToSecondInstructor();

            var data = new AddQuestionCommand()
            {
                MultiVote = "true",
                CourseId = "CourseId",
                QuestionDescription = "description",
                Answers = new List<string>()
                {
                   "Answer1",
                   "Answer2"
                }
            };
            //Act
            var response = await client.PostAsync(_path, data);
            
            //Output
            _outputHelper.WriteLine(await response.GetContent());
            
            //Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            Assert.True(await response.HasErrorCode(ErrorType.Unauthorized));
        }
        
    }
}