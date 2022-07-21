using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.DTOs.User;
using Application.Features.User.Commands.AddUser;
using Application.Features.User.Commands.DeleteUser;
using Application.Features.User.Commands.EditUser;
using Application.Features.User.Queries.GetUserId;
using Application.Features.User.Queries.SearchAllEvents;
using Application.Features.User.Queries.SearchProfile;
using Application.Features.User.Queries.SearchStudent;
using Application.Features.User.Queries.SearchUser;
using Domain.Enum;
using IntegrationTest.Handlers;
using MarkopTest;
using MarkopTest.Attributes;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Controller;

[Endpoint("[controller]/[action]")]
public class UserTests : AppFactory
{
    public UserTests(ITestOutputHelper outputHelper, HttpClient client = null) : base(outputHelper, client)
    {
    }

    [Theory]
    [OwnerHandler]
    [InlineData("PROfficer@Test.app", UserType.PROfficer, null, null)]
    [InlineData("Student@Test.app", UserType.Student, "TestStudentID", null)]
    [InlineData("Instructor@Test.app", UserType.Instructor, null, "TestInstructorID")]
    [InlineData("User@Test.app", UserType.Instructor, null, "12345", HttpStatusCode.NotAcceptable,
        ErrorType.DuplicateUser)]
    [InlineData("User@Test.app", UserType.Student, "12345", null, HttpStatusCode.NotAcceptable,
        ErrorType.DuplicateUser)]
    [InlineData("Student@Farakhu.app", UserType.Student, "12345", null, HttpStatusCode.NotAcceptable,
        ErrorType.DuplicateUser)]
    public async Task AddUser(string email, UserType userType, string studentId, string instructorId,
        HttpStatusCode httpStatusCode = HttpStatusCode.OK, ErrorType? errorCode = null)

    {
        string id;
        switch (userType)
        {
            case UserType.Student:
                id = studentId;
                break;
            case UserType.Instructor:
                id = instructorId;
                break;
            default:
                id = "";
                break;
        }

        var data = new AddUserCommand
        {
            Email = email,
            Id = id,
            UserType = userType,
            Password = "TestUserPassword",
            FirstName = "FirstName",
            LastName = "LastName",
            GoogleScholar = "",
            LinkedIn = ""
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Theory]
    [OwnerHandler]
    [InlineData("DeletePROfficerId")]
    [InlineData("DeleteStudentId")]
    [InlineData("DeleteInstructorId")]
    [InlineData("WrongUserId", HttpStatusCode.NotAcceptable, ErrorType.UserNotFound)]
    public async Task DeleteUser(string id, HttpStatusCode httpStatusCode = HttpStatusCode.OK,
        ErrorType? errorCode = null)

    {
        var data = new DeleteUserCommand
        {
            UserId = id
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Theory]
    [OwnerHandler]
    [MemberData(nameof(EditUserDataProvider))]
    public async Task EditUser(string userId, string firstName = null, string lastName = null, string avatarId = null,
        string linkedIn = null,
        string googleScholar = null, List<string> addFavourites = null, List<string> deleteFavourites = null,
        bool deleteAvatar = false,
        HttpStatusCode httpStatusCode = HttpStatusCode.OK, ErrorType? errorCode = null)
    {
        var data = new EditUserCommand
        {
            UserId = userId,
            FirstName = firstName,
            LastName = lastName,
            AvatarId = avatarId,
            LinkedIn = linkedIn,
            GoogleScholar = googleScholar,
            AddFavourites = addFavourites,
            DeleteFavourites = deleteFavourites,
            DeleteAvatar = deleteAvatar
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    public static IEnumerable<object[]> EditUserDataProvider()
    {
        yield return new object[] { "EditUserId", "NewFirstname" };
        yield return new object[] { "EditUserId", null, "NewLastname" };
        yield return new object[] { "EditUserId", null, null, "sad.png" };
        yield return new object[] { "EditUserId", null, null, null, "New LinkedIn" };
        yield return new object[] { "EditUserId", null, null, null, null, "New GoogleScholar" };
        yield return new object[] { "EditUserId", null, null, null, null, null, new List<string> { "New Favourite" } };
        yield return new object[]
            { "EditUserId", null, null, null, null, null, null, new List<string> { "EditUserFavouriteId" } };
        yield return new object[] { "EditUserId", null, null, null, null, null, null, null, true };
        yield return new object[]
        {
            "WrongUserId", null, null, null, null, null, null, null, false, HttpStatusCode.NotAcceptable,
            ErrorType.UserNotFound
        };
        yield return new object[]
        {
            "EditUserId", null, null, "WrongFileId", null, null, null, null, false, HttpStatusCode.NotAcceptable,
            ErrorType.FileNotFound
        };
    }

    [Fact]
    [StudentHandler]
    [Endpoint("[controller]/GetAllEvents")]
    public async Task GetAllEventsForStudent()
    {
        PostJson(new SearchAllEventsQuery(), new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK
        });
    }

    [Fact]
    [InstructorHandler]
    [Endpoint("[controller]/GetAllEvents")]
    public async Task GetAllEventsForInstructor()
    {
        PostJson(new SearchAllEventsQuery(), new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK
        });
    }


    [Fact]
    [StudentHandler]
    public async Task GetUserId()
    {
        PostJson(new GetUserIdQuery(), new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK
        });
    }

    [Theory]
    [OwnerHandler]
    [InlineData("StudentId", HttpStatusCode.OK, null)]
    [InlineData("WrongId", HttpStatusCode.NotAcceptable, ErrorType.UserNotFound)]
    public async Task SearchProfile(string id, HttpStatusCode httpStatusCode, ErrorType? errorCode)
    {
        var data = new SearchProfileQuery
        {
            UserId = id
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }
    

    [Theory]
    [OwnerHandler]
    [InlineData("12345", HttpStatusCode.OK, null)]
    [InlineData("WrongId", HttpStatusCode.NotAcceptable, ErrorType.StudentNotFound)]
    public async Task SearchStudent(string id, HttpStatusCode httpStatusCode, ErrorType? errorCode)
    {
        var data = new SearchStudentQuery()
        {
            StudentId = id
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Theory]
    [OwnerHandler]
    [MemberData(nameof(SearchUserDataProvider))]
    public async Task SearchUser(string firstname, string lastname, string linkedin, string googleScholar,
        UserColumn userColumn = UserColumn.Id, bool orderDirection = false, bool testingOrder = false,
        Func<ProfileDto, IComparable> getProp = null, int start = 0, int step = 25)
    {
        var data = new SearchUserQuery
        {
            FirstName = firstname,
            LastName = lastname,
            LinkedIn = linkedin,
            GoogleScholar = googleScholar,
            UserColumn = userColumn,
            OrderDirection = orderDirection,
            Start = start,
            Step = step
        };
        
        var response = PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK
        });

        var searchResult = (SearchUserViewModel)JObject.Parse(response.GetContent().Result)
            .ToObject(typeof(SearchUserViewModel));

        if (testingOrder)
        {
            Assert.True(
                searchResult?.Users.SequenceEqual(searchResult.Users.OrderBy(getProp).ToList()));
        }
        else
        {
            Assert.True(searchResult?.Users.Count == 1);
            Assert.True(searchResult.Users[0].UserId == "SearchStudentId");
        }
    }

    public static IEnumerable<object[]> SearchUserDataProvider()
    {
        yield return new object[] { "SearchStudent", null, null, null };
        yield return new object[] { null, "SearchStudent", null, null };
        yield return new object[] { null, null, "TestLinkedIn", null };
        yield return new object[] { null, null, null, "TestGoogleScholar" };
        yield return new object[] { null, null, null, null, UserColumn.Id, true, true, (Func<ProfileDto, IComparable>)(c => c.Id) };
        yield return new object[] { null, null, null, null, UserColumn.Firstname, true, true, (Func<ProfileDto, IComparable>)(c => c.FirstName) };
        yield return new object[] { null, null, null, null, UserColumn.Lastname, true, true, (Func<ProfileDto, IComparable>)(c => c.LastName) };
        yield return new object[] { null, null, null, null, UserColumn.LinkedIn, true, true, (Func<ProfileDto, IComparable>)(c => c.LinkedIn) };
        yield return new object[] { null, null, null, null, UserColumn.GoogleScholar, true, true, (Func<ProfileDto, IComparable>)(c => c.GoogleScholar) };
    }
}