using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.DTOs.Event.PersonalEvent;
using Application.Features.Event.Commands.AddEvent;
using Application.Features.Event.Commands.DeleteEvent;
using Application.Features.Event.Commands.EditEvent;
using Application.Features.Event.Queries.GetIncomingEvent;
using Application.Features.Event.Queries.SearchEvent;
using Domain.Enum;
using IntegrationTest.Handlers;
using MarkopTest;
using MarkopTest.Attributes;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Controller;

[Endpoint("[controller]/[action]")]
public class EventTests : AppFactory
{
    public EventTests(ITestOutputHelper outputHelper, HttpClient client = null) : base(outputHelper, client)
    {
    }

    [Theory]
    [InstructorHandler]
    [InlineData("CourseId", "Description", "EventName", HttpStatusCode.OK)]
    [InlineData("WrongCourseId", "Description", "EventName", HttpStatusCode.NotAcceptable,
        ErrorType.CourseNotFound)]
    [InlineData("CourseId", "Description", null, HttpStatusCode.NotAcceptable,
        ErrorType.InvalidInput)]
    public async Task AddEvent(string courseId, string eventDescription, string eventName,
        HttpStatusCode httpStatusCode, ErrorType? errorCode = null)
    {
        var data = new AddEventCommand
        {
            CourseId = courseId,
            EventDescription = eventDescription,
            EventName = eventName,
            EventTime = DateTime.Now
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Theory]
    [InstructorHandler]
    [InlineData("1", HttpStatusCode.OK, null)]
    [InlineData("WrongEventId", HttpStatusCode.NotAcceptable, ErrorType.EventNotFound)]
    public async Task DeleteEvent(string eventId, HttpStatusCode httpStatusCode, ErrorType? errorCode)
    {
        var data = new DeleteEventCommand
        {
            EventId = eventId,
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Fact]
    [SecondInstructorHandler]
    [Endpoint("[controller]/DeleteEvent")]
    public async Task AnotherInstructorCannotDeleteEvent()
    {
        var data = new DeleteEventCommand
        {
            EventId = "2"
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }

    [Theory]
    [InstructorHandler]
    [InlineData("2", "New Description", null, false, "CourseId")]
    [InlineData("2", null, "EventName", false, "CourseId")]
    [InlineData("2", null, null, false, "EditedCourseId")]
    [InlineData("2", null, null, true, "CourseId")]
    [InlineData("WrongCourseEventId", null, null, false, "CourseId", HttpStatusCode.NotAcceptable,
        ErrorType.EventNotFound)]
    public async Task EditEvent(string eventId, string description, string eventName, Boolean isDone, string courseId,
        HttpStatusCode httpStatusCode = HttpStatusCode.OK,
        ErrorType? errorCode = null)
    {
        var data = new EditEventCommand
        {
            EventId = eventId,
            CourseId = courseId,
            EventDescription = description,
            EventName = eventName,
            EventTime = DateTime.Now.ToString(),
            changingIsDone = isDone
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Endpoint("[controller]/EditEvent")]
    [Fact]
    [SecondInstructorHandler]
    public async Task AnotherInstructorCannotEditEvent()
    {
        var data = new EditEventCommand
        {
            EventId = "2",
            EventDescription = "New Description"
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }

    [Theory]
    [InstructorHandler]
    [MemberData(nameof(SearchEventDataProvider))]
    public async Task SearchEvent(string eventId, string eventName, string eventDescription,
        string courseId, DateTime? eventTime,
        EventColumn EventColumn = EventColumn.EventId, bool orderDirection = false,
        bool testingOrder = false, Func<EventShortDto, IComparable> getProp = null, int start = 0,
        int step = 25)
    {
        var data = new SearchEventQuery
        {
            EventId = eventId,
            Description = eventDescription,
            EventName = eventName,
            CourseId = courseId,
            EventTime = eventTime,
            EventColumn = EventColumn,
            OrderDirection = orderDirection,
            Start = start,
            Step = step
        };

        var response = PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK
        });

        var searchResult =
            (SearchEventViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchEventViewModel));


        if (testingOrder)
        {
            Assert.True(
                searchResult?.Event?.SequenceEqual(searchResult.Event.OrderBy(getProp).ToList()));
        }
        else
        {
            Assert.True(searchResult?.Event.Count == 1);
            Assert.True(searchResult.Event[0].EventId == "SearchEventId");
        }
    }

    public static IEnumerable<object[]> SearchEventDataProvider()
    {
        yield return new object[] { "SearchEventId", null, null, null, null };
        yield return new object[] { null, "SearchEventName", null, null, null };
        yield return new object[] { null, null, "SearchEventDescription", null, null };
        yield return new object[] { null, null, null, "SearchCourseId", null };
        yield return new object[] { null, null, null, null, DateTime.MaxValue };
        yield return new object[]
        {
            null, null, null, null, null, EventColumn.EventId, true, true,
            (Func<EventShortDto, IComparable>)(c => c.EventId)
        };
        yield return new object[]
        {
            null, null, null, null, null, EventColumn.EventDescription, true, true,
            (Func<EventShortDto, IComparable>)(c => c.EventDescription)
        };
        yield return new object[]
        {
            null, null, null, null, null, EventColumn.EventName, true, true,
            (Func<EventShortDto, IComparable>)(c => c.EventName)
        };
        yield return new object[]
        {
            null, null, null, null, null, EventColumn.CreationDate, true, true,
            (Func<EventShortDto, IComparable>)(c => c.CreatedDate)
        };
        yield return new object[]
        {
            null, null, null, null, null, EventColumn.EventTime, true, true,
            (Func<EventShortDto, IComparable>)(c => c.EventTime)
        };
    }

    [Fact]
    [InstructorHandler]
    public async Task GetIncomingEvents()
    {
        var data = new GetIncomingEventQuery();
    
        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK,
        });
    }
}