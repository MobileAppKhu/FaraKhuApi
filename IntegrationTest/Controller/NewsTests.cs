using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.DTOs.News;
using Application.Features.Announcement.Queries.SearchAnnouncements;
using Application.Features.News.Commands.AddComment;
using Application.Features.News.Commands.AddNews;
using Application.Features.News.Commands.CommentApproval;
using Application.Features.News.Commands.DeleteNews;
using Application.Features.News.Commands.EditNews;
using Application.Features.News.Commands.RemoveComment;
using Application.Features.News.Queries.GetComments;
using Application.Features.News.Queries.SearchNews;
using Domain.Enum;
using IntegrationTest.Handlers;
using MarkopTest;
using MarkopTest.Attributes;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Controller;

[Endpoint("[controller]/[action]")]
public class NewsTests : AppFactory
{
    public NewsTests(ITestOutputHelper outputHelper, HttpClient client = null) : base(outputHelper, client)
    {
    }

    [Theory]
    [PublicRelationOfficerHandler]
    [InlineData("Title", "Description", "smiley.png", HttpStatusCode.OK)]
    [InlineData("Title", "Description", "WrongFileId", HttpStatusCode.NotAcceptable, ErrorType.FileNotFound)]
    public async Task AddNews(string title, string description, string fileId, HttpStatusCode httpStatusCode, ErrorType? errorCode = null)
    {
        var data = new AddNewsCommand
        {
            Description = description,
            Title = title,
            FileId = fileId
        };
    
        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }
    
    [Fact]
    [InstructorHandler]
    [Endpoint("[controller]/AddNews")]
    public async Task InstructorCannotCreateNews()
    {
        var data = new AddNewsCommand
        {
            Description = "Description",
            Title = "Title",
            FileId = "smiley.png"
        };
    
        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.Unauthorized
        });
    }
    
    [Theory]
    [PublicRelationOfficerHandler]
    [InlineData("NewsId", "New Title", null, null)]
    [InlineData("NewsId", null, "New Description", null)]
    [InlineData("NewsId", null, null, "sad.png")]
    [InlineData("WrongNewsId", null, null, "sad.png", HttpStatusCode.NotAcceptable, ErrorType.NewsNotFound)]
    [InlineData("NewsId", null, null, "WrongFileId", HttpStatusCode.NotAcceptable, ErrorType.FileNotFound)]
    public async Task EditNews(string newsId, string title, string description, string fileId,
        HttpStatusCode httpStatusCode = HttpStatusCode.OK, ErrorType? errorCode = null)
    
    {
        var data = new EditNewsCommand
        {
            NewsId = newsId,
            Title = title,
            Description = description,
            FileId = fileId
        };
    
        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }
    
    
    [Fact]
    [InstructorHandler]
    [Endpoint("[controller]/EditNews")]
    public async Task InstructorCannotEditNews()
    {
        var data = new EditNewsCommand
        {
            NewsId = "NewsId",
            Description = "Description",
            Title = "Title",
            FileId = "smiley.png"
        };
    
        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.Unauthorized
        });
    }
    
    [Theory]
    [PublicRelationOfficerHandler]
    [InlineData("DeleteNewsId", HttpStatusCode.OK, null)]
    [InlineData("WrongNewsId", HttpStatusCode.NotAcceptable, ErrorType.NewsNotFound)]
    public async Task DeleteNews(string newsId, HttpStatusCode httpStatusCode, ErrorType? errorCode)
    {
        var data = new DeleteNewsCommand
        {
            NewsId = newsId
        };
    
        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }
    
    
    [Fact]
    [InstructorHandler]
    [Endpoint("[controller]/DeleteNews")]
    public async Task InstructorCannotDeleteNews()
    {
        var data = new EditNewsCommand
        {
            NewsId = "NewsId",
            Description = "Description",
            Title = "Title",
            FileId = "smiley.png"
        };
    
        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.Unauthorized
        });
    }
    
    [Theory]
    [PublicRelationOfficerHandler]
    [InlineData("NewsId", "CommentText", null)]
    [InlineData("NewsId", "ReplyText", "CommentId")]
    [InlineData("WrongNewsId", "CommentText", null, HttpStatusCode.NotAcceptable, ErrorType.NewsNotFound)]
    [InlineData("NewsId", "ReplyText", "WrongCommentId", HttpStatusCode.NotAcceptable, ErrorType.CommentNotFound)]
    public async Task AddComment(string newsId, string text, string parentId, HttpStatusCode httpStatusCode = HttpStatusCode.OK, ErrorType? errorCode = null)
    {
        var data = new AddCommentCommand
        {
            NewsId = newsId,
            Text = text,
            ParentId = parentId
        };
    
        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Theory]
    [StudentHandler]
    [InlineData("RemoveCommentId", HttpStatusCode.OK, null)]
    [InlineData("WrongCommentId", HttpStatusCode.NotAcceptable, ErrorType.CommentNotFound)]
    public async Task RemoveComment(string commentId, HttpStatusCode httpStatusCode, ErrorType? errorCode)
    {
        var data = new RemoveCommentCommand
        {
            CommentId = commentId
        };
    
        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }
    
    [Fact]
    [SecondStudentHandler]
    [Endpoint("[controller]/RemoveComment")]
    public async Task AnotherUserCannotDeleteComment()
    {
        var data = new RemoveCommentCommand
        {
            CommentId = "CommentId"
        };
    
        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }

    [Theory]
    [OwnerHandler]
    [InlineData("CommentId", HttpStatusCode.OK, null)]
    [InlineData("WrongCommentId", HttpStatusCode.NotAcceptable, ErrorType.CommentNotFound)]
    public async Task CommentApproval(string commentId, HttpStatusCode httpStatusCode, ErrorType? errorCode)
    {
        var data = new CommentApprovalCommand
        {
            CommentId = commentId,
            Status = CommentStatus.Approved
        };
    
        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Theory]
    [OwnerHandler]
    [InlineData(CommentQueryOption.ByUser, true, null)]
    [InlineData(CommentQueryOption.ByNews, true, "TestNewsId")]
    [InlineData(CommentQueryOption.All, false, null)]
    public async Task GetComments(CommentQueryOption commentQueryOption, bool onlyUnapproved, string newsId)
    {
        var data = new CommentsQuery
        {
            Option = commentQueryOption,
            OnlyUnapproved = onlyUnapproved,
            NewsId = newsId
        };
    
        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK
        });
    }
    
    [Theory]
    [StudentHandler]
    [MemberData(nameof(SearchNewsDataProvider))]
    public async Task SearchNews(string search,
        NewsColumn newsColumn = NewsColumn.NewsId, bool orderDirection = false,
        bool testingOrder = false, Func<NewsDto, IComparable> getProp = null, int start = 0,
        int step = 25)
    {
        var data = new SearchNewsQuery()
        {
            Search = search,
            NewsColumn = newsColumn,
            OrderDirection = orderDirection,
            Start = start,
            Step = step
        };
    
        var response = PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK
        });
    
        var searchResult = (SearchNewsViewModel)JObject.Parse(response.GetContent().Result)
            .ToObject(typeof(SearchNewsViewModel));
    
        if (testingOrder)
        {
            Assert.True(
                searchResult?.News.SequenceEqual(searchResult.News.OrderBy(getProp).ToList()));
        }
        else
        {
            Assert.True(searchResult?.News.Count == 1);
            Assert.True(searchResult.News[0].NewsId == "SearchNewsId");
        }
    
    }
    
    public static IEnumerable<object[]> SearchNewsDataProvider()
    {
        yield return new object[] { "search"};
        yield return new object[] { null, NewsColumn.NewsId, true, true, (Func<NewsDto, IComparable>)(c => c.NewsId)};
        yield return new object[] { null, NewsColumn.Title, true, true, (Func<NewsDto, IComparable>)(c => c.Title)};
        yield return new object[] { null, NewsColumn.Description, true, true, (Func<NewsDto, IComparable>)(c => c.Description)};
        yield return new object[] { null, NewsColumn.CreationDate, true, true, (Func<NewsDto, IComparable>)(c => c.CreatedDate)};
    }

}