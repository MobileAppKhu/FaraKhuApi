using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.DTOs.Faculty;
using Application.Features.Faculty.Queries;
using Domain.Enum;
using IntegrationTest.Handlers;
using MarkopTest;
using MarkopTest.Attributes;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Controller;

[Endpoint("[controller]/[action]")]

public class FacultyTests : AppFactory
{
    public FacultyTests(ITestOutputHelper outputHelper, HttpClient client = null) : base(outputHelper, client)
    {
    }
    
    [Theory]
    [InstructorHandler]
    [MemberData(nameof(SearchFacultyDataProvider))]
    public async Task SearchFaculty(string facultyId, string facultyCode, string facultyTitle,
        FacultyColumn facultyColumn = FacultyColumn.FacultyId, bool orderDirection = false,
        bool testingOrder = false, Func<FacultySearchDto, IComparable> getProp = null, int start = 0,
        int step = 25)
    {
        var data = new SearchFacultyQuery
        {
            FacultyCode = facultyCode,
            FacultyId = facultyId,
            FacultyTitle = facultyTitle,
            FacultyColumn = facultyColumn,
            OrderDirection = orderDirection,
            Start = start,
            Step = step
        };

        var response = PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK
        });

        var searchResult = (SearchFacultyViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchFacultyViewModel));


        if (testingOrder)
        {
            Assert.True(
                searchResult?.Faculties?.SequenceEqual(searchResult.Faculties.OrderBy(getProp).ToList()));
        }
        else
        {
            Assert.True(searchResult?.Faculties.Count == 1);
            Assert.True(searchResult.Faculties[0].FacultyId == "FirstFacultyId");
        }
    }

    public static IEnumerable<object[]> SearchFacultyDataProvider()
    {
        yield return new object[] { "FirstFacultyId", null, null };
        yield return new object[] { null, "1", null};
        yield return new object[] { null, null, "فنی و مهندسی" };
        yield return new object[]
        {
            null, null, null, EventColumn.EventId, true, true,
            (Func<FacultySearchDto, IComparable>)(c => c.FacultyId)
        };
        yield return new object[]
        {
            null, null, null, EventColumn.EventDescription, true, true,
            (Func<FacultySearchDto, IComparable>)(c => c.FacultyCode)
        };
        yield return new object[]
        {
            null, null, null, EventColumn.EventName, true, true,
            (Func<FacultySearchDto, IComparable>)(c => c.FacultyTitle)
        };
    }
}