using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.DTOs.Ticket;
using Application.Features.Ticket.Commands.AddTicket;
using Application.Features.Ticket.Commands.DeleteTicket;
using Application.Features.Ticket.Commands.EditTicket;
using Application.Features.Ticket.Queries.SearchTicket;
using Domain.Enum;
using IntegrationTest.Handlers;
using MarkopTest;
using MarkopTest.Attributes;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Controller;

[Endpoint("[controller]/[action]")]

public class TicketTests : AppFactory
{
    public TicketTests(ITestOutputHelper outputHelper, HttpClient client = null) : base(outputHelper, client)
    {
    }
    
    [Theory]
    [InstructorHandler]
    [InlineData("Description", TicketPriority.Important,  HttpStatusCode.OK)]
    [InlineData(null, TicketPriority.Important,  HttpStatusCode.NotAcceptable,
        ErrorType.InvalidInput)]
    [InlineData("Description", null, HttpStatusCode.NotAcceptable,
        ErrorType.InvalidInput)]
    public async Task AddTicket(string description, TicketPriority priority,
        HttpStatusCode httpStatusCode, ErrorType? errorCode = null)
    {
        var data = new AddTicketCommand
        {
            Description = description,
            Priority = priority
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }
    
    [Theory]
    [InstructorHandler]
    [InlineData("DeleteTicketId", HttpStatusCode.OK)]
    [InlineData("WrongTicketId", HttpStatusCode.NotAcceptable,
        ErrorType.TicketNotFound)]
    public async Task DeleteTicket(string ticketId,
        HttpStatusCode httpStatusCode, ErrorType? errorCode = null)
    {
        var data = new DeleteTicketCommand
        {
            TicketId = ticketId
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }
    
    [Fact]
    [SecondInstructorHandler]
    [Endpoint("[controller]/DeleteTicket")]
    public async Task AnotherInstructorCannotDeleteTicket()
    {
        var data = new DeleteTicketCommand
        {
            TicketId = "SecondTicketId"
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }
    
    [Theory]
    [OwnerHandler]
    [InlineData("EditedDescription", null, null, "SecondTicketId", HttpStatusCode.OK)]
    [InlineData(null, TicketPriority.Normal, null, "SecondTicketId", HttpStatusCode.OK)]
    [InlineData(null, null, TicketStatus.Solved, "SecondTicketId", HttpStatusCode.OK)]
    [InlineData(null, null, TicketStatus.InProgress, "SecondTicketId", HttpStatusCode.OK)]
    [InlineData(null, null, null, "WrongTicketId", HttpStatusCode.NotAcceptable,
        ErrorType.TicketNotFound)]
    public async Task EditTicket(string description, TicketPriority priority, TicketStatus ticketStatus,
        string ticketId, HttpStatusCode httpStatusCode, ErrorType? errorCode = null)
    {
        var data = new EditTicketCommand
        {
            DeadLine = DateTime.Today,
            Description = description,
            Priority = priority,
            TicketStatus = ticketStatus,
            TicketId = ticketId
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }
    
    [Fact]
    [SecondInstructorHandler]
    [Endpoint("[controller]/EditTicket")]
    public async Task AnotherInstructorCannotEditTicket()
    {
        var data = new EditTicketCommand
        {
            TicketId = "SecondTicketId",
            Description = "EditedDescription"
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }
    
    [Fact]
    [InstructorHandler]
    [Endpoint("[controller]/EditTicket")]
    public async Task InstructorCannotEditTicketStatus()
    {
        var data = new EditTicketCommand
        {
            TicketId = "SecondTicketId",
            TicketStatus = TicketStatus.Solved
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }
    
    [Theory]
    [InstructorHandler]
    [MemberData(nameof(SearchTicketDataProvider))]
    public async Task SearchTicket(string ticketId, TicketStatus ticketStatus, TicketPriority ticketPriority,
        string description,
        TicketColumn TicketColumn = TicketColumn.TicketId, bool orderDirection = false,
        bool testingOrder = false, Func<TicketDto, IComparable> getProp = null, int start = 0,
        int step = 25)
    {
        var data = new SearchTicketQuery
        {
            Description = description,
            TicketId = ticketId,
            TicketStatus = ticketStatus,
            TicketPriority = ticketPriority,
            TicketColumn = TicketColumn,
            OrderDirection = orderDirection,
            Start = start,
            Step = step
        };

        var response = PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK
        });

        var searchResult = (SearchTicketQueryViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchTicketQueryViewModel));


        if (testingOrder)
        {
            Assert.True(
                searchResult?.TicketDtos?.SequenceEqual(searchResult.TicketDtos.OrderBy(getProp).ToList()));
        }
        else
        {
            Assert.True(searchResult?.TicketDtos.Count == 1);
            Assert.True(searchResult.TicketDtos[0].TicketId == "SearchTicketId");
        }
    }

    public static IEnumerable<object[]> SearchTicketDataProvider()
    {
        yield return new object[] {"SearchTicketId", null, null, null};
        yield return new object[] {null, TicketStatus.InProgress, null, null};
        yield return new object[] {null, null, TicketPriority.Urgent, null};
        yield return new object[] {null, null, null, "SearchDescription"};
        yield return new object[]
        {
            null, null, null, null, TicketColumn.Description, true, true,
            (Func<TicketDto, IComparable>)(c => c.Description)
        };
        yield return new object[]
        {
            null, null, null, null, TicketColumn.Priority, true, true,
            (Func<TicketDto, IComparable>)(c => c.Priority)
        };
        yield return new object[]
        {
            null, null, null, null, TicketColumn.Status, true, true,
            (Func<TicketDto, IComparable>)(c => c.Status)
        };
        yield return new object[]
        {
            null, null, null, null, TicketColumn.TicketId, true, true,
            (Func<TicketDto, IComparable>)(c => c.TicketId)
        };
        yield return new object[]
        {
            null, null, null, null, TicketColumn.CreationDate, true, true,
            (Func<TicketDto, IComparable>)(c => c.CreatedDate)
        };
        yield return new object[]
        {
            null, null, null, null, TicketColumn.DeadLine, true, true,
            (Func<TicketDto, IComparable>)(c => c.DeadLine)
        };
        
    }

}