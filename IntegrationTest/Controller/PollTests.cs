using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Features.Poll.Commands.AddQuestion;
using Application.Features.Poll.Commands.DeleteQuestion;
using Application.Features.Poll.Commands.EditQuestion;
using Application.Features.Poll.Commands.RetractVote;
using Application.Features.Poll.Commands.Vote;
using Application.Features.Poll.Queries.SearchAvailablePolls;
using Application.Features.Poll.Queries.SearchPoll;
using Domain.Enum;
using IntegrationTest.Handlers;
using MarkopTest.Attributes;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Controller;

[Endpoint("[controller]/[action]")]
public class PollTests : AppFactory
{
    public PollTests(ITestOutputHelper outputHelper, HttpClient client = null) : base(outputHelper, client)
    {
    }

    [Theory]
    [InstructorHandler]
    [MemberData(nameof(AddQuestionDataProvider))]
    public async Task AddPollQuestion(string questionDescription, string courseId, string multiVote,
        List<string> answers,
        HttpStatusCode httpStatusCode = HttpStatusCode.OK, ErrorType? errorCode = null)
    {
        var data = new AddQuestionCommand
        {
            QuestionDescription = questionDescription,
            CourseId = courseId,
            MultiVote = multiVote,
            Answers = answers
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    public static IEnumerable<object[]> AddQuestionDataProvider()
    {
        yield return new object[] { "Description", "CourseId", "true", new List<string> { "Answer1", "Answer2" } };
        yield return new object[]
        {
            "Description", null, "true", new List<string> { "Answer1", "Answer2" }, HttpStatusCode.NotAcceptable,
            ErrorType.InvalidInput
        };
        yield return new object[]
            { "Description", "CourseId", "true", null, HttpStatusCode.NotAcceptable, ErrorType.InvalidInput };
        yield return new object[]
        {
            null, "CourseId", "true", new List<string> { "Answer1", "Answer2" }, HttpStatusCode.NotAcceptable,
            ErrorType.InvalidInput
        };
    }

    [Fact]
    [SecondInstructorHandler]
    [Endpoint("[controller]/AddPollQuestion")]
    public async Task AnotherUserCannotAddQuestion()
    {
        var data = new AddQuestionCommand
        {
            CourseId = "CourseId",
            QuestionDescription = "Description",
            MultiVote = "true",
            Answers = new List<string>
            {
                "Answer1",
                "Answer2"
            }
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }

    [Theory]
    [InstructorHandler]
    [MemberData(nameof(EditQuestionDataProvider))]
    public async Task EditPollQuestion(string questionId, string questionDescription = null,
        string multiVote = null, string isOpen = null, List<string> addAnswers = null,
        List<string> deleteAnswers = null, HttpStatusCode httpStatusCode = HttpStatusCode.OK,
        ErrorType? errorCode = null)
    {
        var data = new EditQuestionCommand
        {
            QuestionId = questionId,
            QuestionDescription = questionDescription,
            MultiVote = multiVote,
            IsOpen = isOpen,
            AddAnswers = addAnswers,
            DeleteAnswers = deleteAnswers
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    public static IEnumerable<object[]> EditQuestionDataProvider()
    {
        yield return new object[] { "SecondQuestionId", "EditedDescription" };
        yield return new object[] { "SecondQuestionId", null, "false" };
        yield return new object[] { "SecondQuestionId", null, null, "false" };
        yield return new object[]
        {
            "SecondQuestionId", null, null, null, new List<string>
            {
                "EditedAnswer"
            }
        };
        yield return new object[]
        {
            "SecondQuestionId", null, null, null, null, new List<string>
            {
                "Answer1"
            }
        };
        yield return new object[]
        {
            "WrongQuestionId", null, null, null, null, null, HttpStatusCode.NotAcceptable, ErrorType.QuestionNotFound
        };
    }

    [Fact]
    [SecondInstructorHandler]
    [Endpoint("[controller]/EditPollQuestion")]
    public async Task AnotherUserCannotEditPollQuestion()
    {
        var data = new EditQuestionCommand
        {
            QuestionId = "SecondQuestionId",
            QuestionDescription = "EditedDescription"
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }

    [Theory]
    [InstructorHandler]
    [InlineData("DeleteQuestionId", HttpStatusCode.OK, null)]
    [InlineData("WrongQuestionId", HttpStatusCode.NotAcceptable, ErrorType.QuestionNotFound)]
    public async Task DeletePollQuestion(string questionId, HttpStatusCode httpStatusCode, ErrorType? errorCode)
    {
        var data = new DeleteQuestionCommand
        {
            QuestionId = questionId
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Fact]
    [SecondInstructorHandler]
    [Endpoint("[controller]/DeletePollQuestion")]
    public async Task AnotherUserCannotDeletePollQuestion()
    {
        var data = new DeleteQuestionCommand
        {
            QuestionId = "SecondQuestionId"
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }

    [Theory]
    [SecondStudentHandler]
    [InlineData("Answer3", HttpStatusCode.OK, null)]
    [InlineData("WrongAnswerId", HttpStatusCode.NotAcceptable, ErrorType.AnswerNotFound)]
    [InlineData("Answer2", HttpStatusCode.NotAcceptable, ErrorType.PollIsNotOpen)]
    public async Task Vote(string answerId, HttpStatusCode httpStatusCode, ErrorType? errorCode)
    {
        var data = new VoteCommand
        {
            AnswerId = answerId
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Fact]
    [StudentHandler]
    [Endpoint("[controller]/Vote")]
    public async Task UserWithoutCourseCanNotVote()
    {
        var data = new VoteCommand
        {
            AnswerId = "Answer3"
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }

    [Theory]
    [SecondStudentHandler]
    [InlineData("Answer3", HttpStatusCode.OK, null)]
    [InlineData("Answer2", HttpStatusCode.NotAcceptable, ErrorType.PollIsNotOpen)]
    public async Task RetractVote(string answerId, HttpStatusCode httpStatusCode, ErrorType? errorCode)
    {
        var data = new RetractVoteCommand
        {
            AnswerId = answerId
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Fact]
    [SecondInstructorHandler]
    public async Task SearchPoll()
    {
        var data = new SearchPollQuery
        {
            QuestionId = "SearchQuestionId"
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK
        });
    }

    [Fact]
    [SecondStudentHandler]
    public async Task SearchAvailablePolls()
    {
        var data = new SearchPollsQuery()
        {
            CourseId = "SearchCourseId",
            Start = 0,
            Step = 25
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK
        });
    }
}