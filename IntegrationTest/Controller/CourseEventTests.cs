using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.DTOs.CourseEvent;
using Application.Features.CourseEvent.Commands.AddCourseEvent;
using Application.Features.CourseEvent.Commands.DeleteCourseEvent;
using Application.Features.CourseEvent.Commands.EditCourseEvent;
using Application.Features.CourseEvent.Queries.SearchCourseEvent;
using Domain.Enum;
using Domain.Models;
using IntegrationTest.Handlers;
using MarkopTest;
using MarkopTest.Attributes;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Controller;

[Endpoint("[controller]/[action]")]
public class CourseEventTests : AppFactory
{
    public CourseEventTests(ITestOutputHelper outputHelper, HttpClient client = null) : base(outputHelper, client)
    {
    }

    [Theory]
    [InstructorHandler]
    [InlineData("CourseId", "Description", "EventName", CourseEventType.Assignment, HttpStatusCode.OK)]
    [InlineData("WrongCourseId", "Description", "EventName", CourseEventType.Assignment, HttpStatusCode.NotAcceptable,
        ErrorType.CourseNotFound)]
    public async Task AddCourseEvent(string courseId, string eventDescription, string eventName,
        CourseEventType eventType, HttpStatusCode httpStatusCode, ErrorType? errorCode = null)
    {
        var data = new AddCourseEventCommand
        {
            CourseId = courseId,
            EventDescription = eventDescription,
            EventName = eventName,
            EventTime = DateTime.Now,
            EventType = eventType
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Fact]
    [Endpoint("[controller]/AddCourseEvent")]
    [SecondInstructorHandler]
    public async Task AnotherInstructorCannotAddCourseEvent()
    {
        var data = new AddCourseEventCommand
        {
            CourseId = "CourseId",
            EventDescription = "Description",
            EventName = "EventName",
            EventTime = DateTime.Now,
            EventType = CourseEventType.Assignment
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }

    [Theory]
    [InstructorHandler]
    [InlineData("2", "New Description", null, null)]
    [InlineData("2", null, "EventName", null)]
    [InlineData("2", null, null, CourseEventType.Exam)]
    [InlineData("WrongCourseEventId", null, null, null, HttpStatusCode.NotAcceptable, ErrorType.CourseEventNotFound)]
    public async Task EditCourseEvent(string courseEventId, string description, string eventName,
        CourseEventType? courseEventType, HttpStatusCode httpStatusCode = HttpStatusCode.OK,
        ErrorType? errorCode = null)
    {
        var data = new EditCourseEventCommand
        {
            CourseEventId = courseEventId,
            Description = description,
            EventName = eventName,
            CourseEventType = courseEventType,
            EventTime = DateTime.Now.ToString()
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK,
            ErrorCode = errorCode
        });
    }

    [Endpoint("[controller]/EditCourseEvent")]
    [Fact]
    [SecondInstructorHandler]
    public async Task AnotherInstructorCannotEdit()
    {
        var data = new EditCourseEventCommand
        {
            CourseEventId = "2",
            Description = "New Description"
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }

    [Theory]
    [InstructorHandler]
    [InlineData("1", HttpStatusCode.OK, null)]
    [InlineData("WrongCourseEventId", HttpStatusCode.NotAcceptable, ErrorType.CourseEventNotFound)]
    public async Task DeleteCourseEvent(string courseEventId, HttpStatusCode httpStatusCode, ErrorType? errorCode)
    {
        var data = new DeleteCourseEventCommand
        {
            CourseEventId = courseEventId,
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Fact]
    [SecondInstructorHandler]
    [Endpoint("[controller]/DeleteCourseEvent")]
    public async Task AnotherInstructorCannotDeleteCourseEvent()
    {
        var data = new DeleteCourseEventCommand
        {
            CourseEventId = "2"
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }

    [Theory]
    [InstructorHandler]
    [MemberData(nameof(SearchCourseEventDataProvider))]
    public async Task SearchCourseEvent(string courseEventId, string eventName, string eventDescription, string courseId,
        CourseEventColumn courseEventColumn = CourseEventColumn.CourseId, bool orderDirection = false,
        bool testingOrder = false, Func<SearchCourseCourseEventDto, IComparable> getProp = null, int start = 0,
        int step = 25)
    {
        var data = new SearchCourseEventQuery
        {
            CourseEventId = courseEventId,
            EventDescription = eventDescription,
            EventName = eventName,
            CourseId = courseId,
            CourseEventColumn = courseEventColumn,
            OrderDirection = orderDirection,
            Start = start,
            Step = step
        };

        var response = PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK
        });

        var searchResult = (SearchCourseEventViewModel)JObject.Parse(response.GetContent().Result)
            .ToObject(typeof(SearchCourseEventViewModel));

        if (testingOrder)
        {
            Assert.True(
                searchResult?.CourseEvents?.SequenceEqual(searchResult.CourseEvents.OrderBy(getProp).ToList()));
        }
        else
        {
            Assert.True(searchResult?.CourseEvents.Count == 1);
            Assert.True(searchResult.CourseEvents[0].CourseEventId == "3");
        }
    }

    public static IEnumerable<object[]> SearchCourseEventDataProvider()
    {
        yield return new object[] { "3", null, null, null };
        yield return new object[] { null, "SearchEventName", null, null };
        yield return new object[] { null, null, "SearchDescription", null };
        yield return new object[] { null, null, null, "SearchCourseId" };
        yield return new object[]
        {
            null, null, null, null, CourseEventColumn.CourseEventId, true, true,
            (Func<SearchCourseCourseEventDto, IComparable>)(c => c.CourseEventId)
        };
        yield return new object[]
        {
            null, null, null, null, CourseEventColumn.CourseEventDescription, true, true,
            (Func<SearchCourseCourseEventDto, IComparable>)(c => c.Description)
        };
        yield return new object[]
        {
            null, null, null, null, CourseEventColumn.CourseEventName, true, true,
            (Func<SearchCourseCourseEventDto, IComparable>)(c => c.EventName)
        };
        yield return new object[]
        {
            null, null, null, null, CourseEventColumn.CourseId, true, true,
            (Func<SearchCourseCourseEventDto, IComparable>)(c => c.CourseId)
        };
    }
}