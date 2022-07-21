using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Application.DTOs.Announcement;
using Application.Features.Announcement.Commands.AddAnnouncement;
using Application.Features.Announcement.Commands.DeleteAnnouncement;
using Application.Features.Announcement.Commands.EditAnnouncement;
using Application.Features.Announcement.Queries.SearchAnnouncements;
using Domain.Enum;
using IntegrationTest.Handlers;
using MarkopTest;
using MarkopTest.Attributes;
using MarkopTest.IntegrationTest;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Controller;

[Endpoint("[controller]/[action]")]
public class AnnouncementTests : AppFactory
{
    public AnnouncementTests(ITestOutputHelper outputHelper, HttpClient client = null) : base(outputHelper, client)
    {
    }

    [Theory]
    [InstructorHandler]
    [InlineData("smiley.png", "test description", "test title")]
    [InlineData("wrongFileId", "test description", "test title", HttpStatusCode.NotAcceptable, ErrorType.FileNotFound)]
    [InlineData(null, "test description", "test title")]
    public async Task AddAnnouncement(string? avatar, string? description, string? title,
        HttpStatusCode httpStatusCode = HttpStatusCode.OK, ErrorType? errorCode = null)
    {
        var data = new AddAnnouncementCommand
        {
            Avatar = avatar,
            Description = description,
            Title = title
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.Unauthorized
        },
        new TestHandlerOptions {
            BeforeRequest = false
        });
    }

    [Theory]
    [StudentHandler]
    [InlineData("EditAnnouncement", "New Description", null, null)]
    [InlineData("EditAnnouncement", null, "New Title", null)]
    [InlineData("EditAnnouncement", null, null, "sad.png")]
    [InlineData("EditAnnouncement", null, null, "WrongFileId", HttpStatusCode.NotAcceptable, ErrorType.FileNotFound)]
    public async Task EditAnnouncement(string announcementId, string description, string title, string avatarId,
        HttpStatusCode httpStatusCode = HttpStatusCode.OK, ErrorType? errorCode = null)
    {
        var data = new EditAnnouncementCommand
        {
            AnnouncementId = announcementId,
            Description = description,
            Title = title,
            AvatarId = avatarId
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Endpoint("[controller]/EditAnnouncement")]
    [SecondStudentHandler]
    [Fact]
    public async Task AnotherUserCannotEdit()
    {
        var data = new EditAnnouncementCommand
        {
            AnnouncementId = "EditAnnouncement",
            Title = "New Title"
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.Unauthorized,
            ErrorCode = ErrorType.Unauthorized
        });
    }

    [Theory]
    [StudentHandler]
    [MemberData(nameof(SearchAnnouncementDataProvider))]
    public async Task SearchAnnouncements(string announcementId, string description, string title, string user,
        AnnouncementColumn announcementColumn = AnnouncementColumn.AnnouncementId, bool orderDirection = false,
        bool testingOrder = false, Func<SearchAnnouncementDto, IComparable> getProp = null, int start = 0,
        int step = 25)
    {
        var data = new SearchAnnouncementsQuery
        {
            AnnouncementId = announcementId,
            Description = description,
            Title = title,
            User = user,
            AnnouncementColumn = announcementColumn,
            OrderDirection = orderDirection,
            Start = start,
            Step = step
        };

        var response = PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK
        });

        var searchResult = (SearchAnnouncementsViewModel)JObject.Parse(response.GetContent().Result)
            .ToObject(typeof(SearchAnnouncementsViewModel));

        if (testingOrder)
        {
            Assert.True(
                searchResult?.Announcements.SequenceEqual(searchResult.Announcements.OrderBy(getProp).ToList()));
        }
        else
        {
            Assert.True(searchResult?.Announcements.Count == 1);
            Assert.True(searchResult.Announcements[0].AnnouncementId == "SearchAnnouncement");
        }

    }

    public static IEnumerable<object[]> SearchAnnouncementDataProvider()
    {
        yield return new object[] { "SearchAnnouncement", null, null, null };
        yield return new object[] { null, "Description2", null, null };
        yield return new object[] { null, null, "Title2", null };
        yield return new object[] { null, null, null, "SecondStudentId" };
        yield return new object[]
        {
            null, null, null, null, AnnouncementColumn.Description, true, true,
            (Func<SearchAnnouncementDto, IComparable>)(c => c.AnnouncementDescription),
        };
        yield return new object[]
        {
            null, null, null, null, AnnouncementColumn.Title, true, true,
            (Func<SearchAnnouncementDto, IComparable>)(c => c.AnnouncementTitle),
        };
        yield return new object[]
        {
            null, null, null, null, AnnouncementColumn.CreationDate, true, true,
            (Func<SearchAnnouncementDto, IComparable>)(c => c.CreatedDate),
        };
        yield return new object[]
        {
            null, null, null, null, AnnouncementColumn.UserId, true, true,
            (Func<SearchAnnouncementDto, IComparable>)(c => c.UserId),
        };
    }

    [Theory]
    [StudentHandler]
    [InlineData("DeleteAnnouncement", HttpStatusCode.OK, null)]
    [InlineData("WrongAnnouncementId", HttpStatusCode.NotAcceptable, ErrorType.AnnouncementNotFound)]
    public async Task DeleteAnnouncement(string announcementId, HttpStatusCode httpStatusCode, ErrorType? errorCode)
    {
        var data = new DeleteAnnouncementCommand
        {
            AnnouncementId = announcementId
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Fact]
    [InstructorHandler]
    [Endpoint("[controller]/DeleteAnnouncement")]
    public async Task AnotherUserCannotDeleteAnnouncement()
    {
        var data = new DeleteAnnouncementCommand
        {
            AnnouncementId = "EditAnnouncement"
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }
}