using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.DTOs.Course;
using Application.DTOs.Student;
using Application.DTOs.Time;
using Application.Features.Course.Commands.AddCourse;
using Application.Features.Course.Commands.DeleteCourse;
using Application.Features.Course.Commands.EditCourse;
using Application.Features.Course.Queries.SearchCourse;
using Domain.Enum;
using IntegrationTest.Handlers;
using MarkopTest;
using MarkopTest.Attributes;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Controller;

[Endpoint("[controller]/[action]")]
public class CourseTests : AppFactory
{
    public CourseTests(ITestOutputHelper outputHelper, HttpClient client = null) : base(outputHelper, client)
    {
    }

    [Theory]
    [InstructorHandler]
    [MemberData(nameof(AddCourseDataProvider))]
    public async Task AddCourse(string courseTypeId, string address, List<AddTimeDto> addTimeDtos,
        AddStudentDto addStudentDto, DateTime endDate, string avatarId,
        HttpStatusCode httpStatusCode, ErrorType? errorCode)
    {
        var data = new AddCourseCommand
        {
            CourseTypeId = courseTypeId,
            Address = address,
            AddTimeDtos = addTimeDtos,
            AddStudentDto = addStudentDto,
            EndDate = endDate,
            AvatarId = avatarId
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Theory]
    [Endpoint("[controller]/AddCourse")]
    [OwnerHandler]
    [InlineData("12345", HttpStatusCode.OK, null)]
    [InlineData("FakeInstructorId", HttpStatusCode.NotAcceptable, ErrorType.InstructorNotFound)]
    public async Task OwnerCreatingCourseForInstructor(string instructorId, HttpStatusCode httpStatusCode,
        ErrorType? errorCode)
    {
        var data = new AddCourseCommand
        {
            EndDate = DateTime.Now,
            AddStudentDto = new AddStudentDto
            {
                StudentIds = new List<string>()
            },
            AddTimeDtos = new List<AddTimeDto>(),
            CourseTypeId = "2",
            Address = "Address",
            AvatarId = "smiley.png",
            InstructorId = instructorId,
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Fact]
    [Endpoint("[controller]/AddCourse")]
    [PublicRelationOfficerHandler]
    public async Task PublicRelationOfficerCannotCreateCourse()
    {
        var data = new AddCourseCommand
        {
            EndDate = DateTime.Now,
            AddStudentDto = new AddStudentDto
            {
                StudentIds = new List<string>()
            },
            AddTimeDtos = new List<AddTimeDto>
            {
                new()
                {
                    StartTime = "15-30",
                    EndTime = "17-30",
                    WeekDay = WeekDay.Monday
                },
                new()
                {
                    StartTime = "16-30",
                    EndTime = "18-30",
                    WeekDay = WeekDay.Monday
                }
            },
            CourseTypeId = "1",
            Address = "Address",
            AvatarId = "smiley.png"
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }

    public static IEnumerable<object[]> AddCourseDataProvider()
    {
        yield return new object[]
        {
            "2", "Test Address", new List<AddTimeDto>
            {
                new()
                {
                    StartTime = "13-40",
                    EndTime = "14-40",
                    WeekDay = WeekDay.Saturday
                }
            },
            new AddStudentDto
            {
                StudentIds = new List<string> { "12345" }
            },
            DateTime.Now,
            "smiley.png",
            HttpStatusCode.OK,
            null
        };

        yield return new object[]
        {
            "2", "Test Address", new List<AddTimeDto>
            {
                new()
                {
                    StartTime = "13-40",
                    EndTime = "14-40",
                    WeekDay = WeekDay.Saturday
                }
            },
            new AddStudentDto
            {
                StudentIds = new List<string> { "12345" }
            },
            DateTime.Now,
            "WrongID",
            HttpStatusCode.NotAcceptable,
            ErrorType.FileNotFound
        };

        yield return new object[]
        {
            "2", "Test Address", new List<AddTimeDto>
            {
                new()
                {
                    StartTime = "13-40",
                    EndTime = "14-40",
                    WeekDay = WeekDay.Saturday
                },
                new()
                {
                    StartTime = "13-50",
                    EndTime = "14-50",
                    WeekDay = WeekDay.Saturday
                }
            },
            new AddStudentDto
            {
                StudentIds = new List<string> { "12345" }
            },
            DateTime.Now,
            "WrongID",
            HttpStatusCode.NotAcceptable,
            ErrorType.TimeConflict
        };

        yield return new object[]
        {
            "2", "Test Address", new List<AddTimeDto>
            {
                new()
                {
                    StartTime = "13-40",
                    EndTime = "14-40",
                    WeekDay = WeekDay.Saturday
                },
                new()
                {
                    StartTime = "13-50",
                    EndTime = "14-50",
                    WeekDay = WeekDay.Saturday
                }
            },
            new AddStudentDto
            {
                StudentIds = new List<string> { "12345" }
            },
            DateTime.Now,
            "smiley.png",
            HttpStatusCode.NotAcceptable,
            ErrorType.TimeConflict
        };

        yield return new object[]
        {
            "2", "Test Address", new List<AddTimeDto>
            {
                new()
                {
                    StartTime = "13-40",
                    EndTime = "14-40",
                    WeekDay = WeekDay.Saturday
                }
            },
            new AddStudentDto
            {
                StudentIds = new List<string> { "WrongStudentID" }
            },
            DateTime.Now,
            "smiley.png",
            HttpStatusCode.NotAcceptable,
            ErrorType.StudentNotFound
        };
    }

    [Theory]
    [InstructorHandler]
    [MemberData(nameof(EditCourseDataProvider))]
    public async Task EditCourse(string courseId, string address = null, DateTime? endDate = null,
        string courseTypeId = null, string avatarId = null, AddStudentDto addStudentDto = null,
        DeleteStudentDto deleteStudentDto = null, List<AddTimeDto> addTimeDtos = null,
        DeleteTimeDto deleteTimeDto = null, HttpStatusCode httpStatusCode = HttpStatusCode.OK,
        ErrorType? errorCode = null)
    {
        var data = new EditCourseCommand
        {
            CourseId = courseId,
            Address = address,
            EndDate = endDate,
            CourseTypeId = courseTypeId,
            AvatarId = avatarId,
            AddStudentDto = addStudentDto,
            DeleteStudentDto = deleteStudentDto,
            AddTimeDtos = addTimeDtos,
            DeleteTimeDto = deleteTimeDto
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    public static IEnumerable<object[]> EditCourseDataProvider()
    {
        yield return new object[] { "EditedCourseId", "NewAddress" };
        yield return new object[] { "EditedCourseId", null, DateTime.Now };
        yield return new object[] { "EditedCourseId", null, null, "3" };
        yield return new object[] { "EditedCourseId", null, null, null, "sad.png" };
        yield return new object[]
        {
            "EditedCourseId", null, null, null, null, null,
            new DeleteStudentDto { StudentIds = new List<string>{ "1234512345" } }
        };
        yield return new object[]
        {
            "EditedCourseId", null, null, null, null, null,
            new DeleteStudentDto { StudentIds = new List<string>{ "WrongStudentId" }},
            null, null, HttpStatusCode.NotAcceptable, ErrorType.StudentNotFound
        };
        yield return new object[]
        {
            "EditedCourseId", null, null, null, null, null,
            new DeleteStudentDto { StudentIds = new List<string>{ "12345" }},
            null, null, HttpStatusCode.NotAcceptable, ErrorType.StudentNotFound
        };
        yield return new object[]
        {
            "EditedCourseId", null, null, null, null,
            new AddStudentDto { StudentIds = new List<string>{ "1234512345" } }
        };
        yield return new object[]
        {
            "EditedCourseId", null, null, null, null, null, null, new List<AddTimeDto>{new ()
            {
                StartTime = "17-30",
                EndTime = "19-30",
                WeekDay = WeekDay.Saturday
            }}, null, HttpStatusCode.NotAcceptable, ErrorType.TimeConflict
        };
        yield return new object[]
        {
            "EditedCourseId", null, null, null, null, null, null, new List<AddTimeDto>{new ()
            {
                StartTime = "17-30",
                EndTime = "19-30",
                WeekDay = WeekDay.Friday
            }}
        };
        yield return new object[]
        {
            "EditedCourseId", null, null, null, null, null, null, null, new DeleteTimeDto{TimeIds = new List<string>
            {
                "SecondTimeId"
            }}
        };
        yield return new object[]
        {
            "EditedCourseId", null, null, null, null, null, null, null, new DeleteTimeDto{TimeIds = new List<string>
            {
                "TimeId"
            }}, HttpStatusCode.NotAcceptable, ErrorType.TimeNotFound
        };
        yield return new object[]
        {
            "EditedCourseId", null, null, null, null, null, null, null, new DeleteTimeDto{TimeIds = new List<string>
            {
                "WrongTimeId"
            }}, HttpStatusCode.NotAcceptable, ErrorType.TimeNotFound
        };
        yield return new object[]
        {
            "WrongCourseId", null, null, null, null, null, null, null, null
            , HttpStatusCode.NotAcceptable, ErrorType.CourseNotFound
        };
    }
    
    [Fact]
    [Endpoint("[controller]/DeleteCourse")]
    [SecondInstructorHandler]
    public async Task DeleteCourseZShouldNotBeFound()
    {
        var data = new DeleteCourseCommand
        {
            CourseId = "DeleteCourseId"
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }
    
    [Theory]
    [InstructorHandler]
    [InlineData("DeleteCourseId", HttpStatusCode.OK, null)]
    [InlineData("WrongCourseId", HttpStatusCode.NotAcceptable, ErrorType.CourseNotFound)]
    public async Task DeleteCourse(string courseId, HttpStatusCode httpStatusCode, ErrorType? errorCode)
    {
        var data = new DeleteCourseCommand
        {
            CourseId = courseId
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Theory]
    [InstructorHandler]
    [MemberData(nameof(SearchCourseDataProvider))]
    public async Task SearchCourse(string courseId, string instructor, string student,
        string courseType, CourseColumn courseColumn = CourseColumn.CourseId, bool orderDirection = true, bool testingOrder = false,
        Func<SearchCourseDto, IComparable> getProp = null, int start = 0, int step = 25)
    {
        var data = new SearchCourseQuery
        {
            CourseId = courseId,
            Instructor = instructor,
            Student = student,
            CourseType = courseType,
            CourseColumn = courseColumn,
            OrderDirection = orderDirection,
            Start = start,
            Step = step
        };

        var response = PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK
        });
        
        var searchResult = (SearchCourseViewModel)JObject.Parse(response.GetContent().Result)
            .ToObject(typeof(SearchCourseViewModel));
        
        if (testingOrder)
        {
            Assert.True(
                searchResult?.Course.SequenceEqual(searchResult.Course.OrderBy(getProp).ToList()));
        }
        else
        {
            Assert.True(searchResult?.Course.Count == 1);
            Assert.True(searchResult.Course[0].CourseId == "SearchCourseId");
        }
    }

    public static IEnumerable<object[]> SearchCourseDataProvider()
    {
        yield return new object[] { "SearchCourseId" , null, null, null};
        yield return new object[] { null, "SecondInstructorId" , null, null};
        yield return new object[] { null, null, "SearchStudentId", null};
        yield return new object[] { null, null, null, "3" };
        yield return new object[]
        {
            null, null, null, null, CourseColumn.CourseId, true, true,
            (Func<SearchCourseDto, IComparable>)(c => c.CourseId)
        };
        yield return new object[]
        {
            null, null, null, null, CourseColumn.CreationDate, true, true,
            (Func<SearchCourseDto, IComparable>)(c => c.CreatedDate)
        };
        yield return new object[]
        {
            null, null, null, null, CourseColumn.EndDate, true, true,
            (Func<SearchCourseDto, IComparable>)(c => c.EndDate)
        };
        yield return new object[]
        {
            null, null, null, null, CourseColumn.InstructorId, true, true,
            (Func<SearchCourseDto, IComparable>)(c => c.InstructorId)
        };
        yield return new object[]
        {
            null, null, null, null, CourseColumn.CourseTypeId, true, true,
            (Func<SearchCourseDto, IComparable>)(c => c.CourseTypeId)
        };
    }
}