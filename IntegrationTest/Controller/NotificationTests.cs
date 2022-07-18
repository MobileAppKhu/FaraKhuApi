using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Features.Notification.Commands.AddCourseNotification;
using Application.Features.Notification.Commands.DeleteNotification;
using Application.Features.Notification.Queries.SearchNotification;
using Domain.Enum;
using IntegrationTest.Handlers;
using MarkopTest.Attributes;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Controller;

[Endpoint("[controller]/[action]")]

public class NotificationTests : AppFactory
{
    public NotificationTests(ITestOutputHelper outputHelper, HttpClient client = null) : base(outputHelper, client)
    {
    }
    
    [Theory]
    [InstructorHandler]
    [InlineData("CourseId", "Description", HttpStatusCode.OK)]
    [InlineData("WrongCourseId", "Description", HttpStatusCode.NotAcceptable,
        ErrorType.CourseNotFound)]
    public async Task AddCourseNotification(string courseId, string description,
        HttpStatusCode httpStatusCode, ErrorType? errorCode = null)
    {
        var data = new AddCourseNotificationCommand
        {
           Description = description,
           CourseId = courseId
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }
    
    [Fact]
    [SecondInstructorHandler]
    [Endpoint("[controller]/AddCourseNotification")]
    public async Task AnotherInstructorCannotAddNotification()
    {
        var data = new AddCourseNotificationCommand
        { 
            Description = "New Notification",
            CourseId = "CourseId"
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }
    
    [Fact]
    [PublicRelationOfficerHandler]
    [Endpoint("[controller]/AddCourseNotification")]
    public async Task PublicRelationAgentCannotAddNotification()
    {
        var data = new AddCourseNotificationCommand
        { 
            Description = "New Notification",
            CourseId = "CourseId"
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }
    
    [Theory]
    [StudentHandler]
    [InlineData("DeleteNotificationId", HttpStatusCode.OK)]
    [InlineData("WrongNotificationId", HttpStatusCode.NotAcceptable,
        ErrorType.NotificationNotFound)]
    public async Task DeleteNotification(string notificationId,
        HttpStatusCode httpStatusCode, ErrorType? errorCode = null)
    {
        var data = new DeleteNotificationCommand
        {
            NotificationId = notificationId
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }
    
    
    [Fact]
    [StudentHandler]
    public async Task SearchNotification(){
        var data = new SearchNotificationQuery();
    
        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK,
        });
    }
}