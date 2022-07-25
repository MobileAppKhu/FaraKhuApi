using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.DTOs.CourseType;
using Application.Features.CourseType.Queries.SearchCourseType;
using Domain.Enum;
using IntegrationTest.Handlers;
using MarkopTest;
using MarkopTest.Attributes;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Controller;

[Endpoint("[controller]/[action]")]

public class CourseTypeTests : AppFactory
{
    public CourseTypeTests(ITestOutputHelper outputHelper, HttpClient client = null) : base(outputHelper, client)
    {
    }
    
    [Theory]
    [InstructorHandler]
    [MemberData(nameof(SearchCourseTypeDataProvider))]
    public async Task SearchCourseType(string CourseTypeId, string CourseTypeCode, string CourseTypeTitle,
        string DepartmentId,
        CourseTypeColumn CourseTypeColumn = CourseTypeColumn.CourseTypeId, bool orderDirection = false,
        bool testingOrder = false, Func<CourseTypeSearchDto, IComparable> getProp = null, int start = 0,
        int step = 25)
    {
        var data = new SearchCourseTypeQuery
        {
            CourseTypeId = CourseTypeId,
            CourseTypeCode = CourseTypeCode,
            CourseTypeTitle = CourseTypeTitle,
            DepartmentId = DepartmentId,
            CourseTypeColumn = CourseTypeColumn,
            OrderDirection = orderDirection,
            Start = start,
            Step = step
        };

        var response = PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK
        });

        var searchResult = (SearchCourseTypeViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchCourseTypeViewModel));


        if (testingOrder)
        {
            Assert.True(
                searchResult?.CourseTypes?.SequenceEqual(searchResult.CourseTypes.OrderBy(getProp).ToList()));
        }
        else
        {
            Assert.True(searchResult?.CourseTypes.Count == 1);
            Assert.True(searchResult.CourseTypes[0].CourseTypeId == "1");
        }
    }

    public static IEnumerable<object[]> SearchCourseTypeDataProvider()
    {
        yield return new object[] { "1", null, null, null};
        yield return new object[] { null, "111", null, null};
        yield return new object[] { null, null, "مبانی کامپیوتر", null};
        yield return new object[] { null, null, null, "FirstDepartmentId"};
        yield return new object[]
        {
            null, null, null, null, CourseTypeColumn.DepartmentId, true, true,
            (Func<CourseTypeSearchDto, IComparable>)(c => c.DepartmentId)
        };
        yield return new object[]
        {
            null, null, null, null, CourseTypeColumn.CourseTypeCode, true, true,
            (Func<CourseTypeSearchDto, IComparable>)(c => c.CourseTypeCode)
        };
        yield return new object[]
        {
            null, null, null, null, CourseTypeColumn.CourseTypeId, true, true,
            (Func<CourseTypeSearchDto, IComparable>)(c => c.CourseTypeId)
        };
        yield return new object[]
        {
            null, null, null, null, CourseTypeColumn.CourseTypeTitle, true, true,
            (Func<CourseTypeSearchDto, IComparable>)(c => c.CourseTypeTitle)
        };
    }
}