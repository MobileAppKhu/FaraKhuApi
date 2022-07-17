﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Features.Account.Commands.ChangePassword;
using Application.Features.Account.Commands.EditProfile;
using Application.Features.Account.Commands.SignIn;
using Application.Features.Account.Commands.SignOut;
using Domain.Enum;
using IntegrationTest.Handlers;
using MarkopTest.Attributes;
using MarkopTest.IntegrationTest;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Controller;

[Endpoint("[controller]/[action]")]
public class AccountTests : AppFactory
{
    public AccountTests(ITestOutputHelper outputHelper, HttpClient client = null) : base(outputHelper, client)
    {
    }

    [Theory]
    [InlineData("Student@Farakhu.app", "StudentPassword")]
    [InlineData("SecondStudent@Farakhu.app", "SecondStudentPassword")]
    [InlineData("Instructor@Farakhu.app", "InstructorPassword")]
    [InlineData("PublicRelation@FaraKhu.app", "PROfficerPassword")]
    [InlineData("Owner@Farakhu.app", "OwnerPassword")]
    [InlineData("Student@Farakhu.app", "NotStudentPassword", HttpStatusCode.NotAcceptable, ErrorType.InvalidInput)]
    [InlineData("Instructor@Farakhu.com", "InstructorPassword", HttpStatusCode.NotAcceptable, ErrorType.UserNotFound)]
    public async Task SignIn(string logon, string password, HttpStatusCode httpStatusCode = HttpStatusCode.OK,
        ErrorType? errorCode = null)
    {
        var data = new SignInCommand
        {
            Logon = logon,
            Password = password
        };

        PostJson(data, new FetchOptions
            {
                HttpStatusCode = httpStatusCode,
                ErrorCode = errorCode
            },
            new TestHandlerOptions
            {
                BeforeRequest = false
            });
    }

    [Theory]
    [StudentHandler]
    [InlineData(null)]
    public void SignOut(ErrorType? errorCode = null)
    {
        var data = new SignOutCommand();

        PostJson(data, new FetchOptions
        {
            ErrorCode = errorCode
        });

        PostJson(data,
            new FetchOptions
            {
                ErrorCode = null,
                HttpStatusCode = HttpStatusCode.Unauthorized
            },
            new TestHandlerOptions
            {
                BeforeRequest = false
            });
    }

    [Theory]
    [StudentHandler]
    [MemberData(nameof(EditProfileDataProvider))]
    public async Task EditProfile(string? firstname, string? lastname, List<string> AddFavourites,
        List<string> DeleteFavourites, string linkedIn = null, string googleScholar = null,
        HttpStatusCode httpStatusCode = HttpStatusCode.OK, ErrorType? errorCode = null)
    {
        var data = new EditProfileCommand()
        {
            FirstName = firstname,
            LastName = lastname,
            AddFavourites = new List<string>(),
            DeleteFavourites = new List<string>(),
            LinkedIn = linkedIn,
            GoogleScholar = googleScholar
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    public static IEnumerable<object[]> EditProfileDataProvider()
    {
        yield return new object[] { "EditedFirstname", null, new List<string>(), new List<string>() };
        yield return new object[] { null, "EditedLastname", new List<string>(), new List<string>() };
        yield return new object[] { null, null, new List<string> { "Java" }, new List<string>() };
        yield return new object[] { null, null, new List<string>(), new List<string>(), "LinkedIn" };
        yield return new object[] { null, null, new List<string>(), new List<string>(), null, "GoogleScholar" };
    }

    [Fact]
    [StudentHandler]
    public async Task ChangePassword()
    {
        var data = new ChangePasswordCommand
        {
            NewPassword = "NewPassword",
            OldPassword = "NotStudentPassword"
        };
        
        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.InvalidPassword
        });
    }
}