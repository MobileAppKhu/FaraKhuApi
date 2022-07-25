using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.DTOs.Department;
using Application.Features.Department.Queries.SearchDepartment;
using Domain.Enum;
using IntegrationTest.Handlers;
using MarkopTest;
using MarkopTest.Attributes;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Controller;

[Endpoint("[controller]/[action]")]
public class DepartmentTests : AppFactory
{
    public DepartmentTests(ITestOutputHelper outputHelper, HttpClient client = null) : base(outputHelper, client)
    {
    }

    [Theory]
    [InstructorHandler]
    [MemberData(nameof(SearchDepartmentDataProvider))]
    public async Task SearchDepartment(string departmentId, string departmentCode, string departmentTitle,
        string facultyId,
        DepartmentColumn departmentColumn = DepartmentColumn.DepartmentId, bool orderDirection = false,
        bool testingOrder = false, Func<DepartmentSearchDto, IComparable> getProp = null, int start = 0,
        int step = 25, int count = 1, bool checkId = true)
    {
        var data = new SearchDepartmentQuery
        {
            DepartmentId = departmentId,
            DepartmentCode = departmentCode,
            DepartmentTitle = departmentTitle,
            FacultyId = facultyId,
            DepartmentColumn = departmentColumn,
            OrderDirection = orderDirection,
            Start = start,
            Step = step
        };

        var response = PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK
        });

        var searchResult = (SearchDepartmentViewModel)JObject.Parse(response.GetContent().Result)
            .ToObject(typeof(SearchDepartmentViewModel));

        if (testingOrder)
        {
            Assert.True(
                searchResult?.Departments.SequenceEqual(searchResult.Departments.OrderBy(getProp).ToList()));
        }
        else
        {
            Assert.True(searchResult?.Departments.Count == count);
            if (checkId)
            {
                Assert.True(searchResult.Departments[0].DepartmentId == "FirstDepartmentId");
            }
        }
    }

    public static IEnumerable<object[]> SearchDepartmentDataProvider()
    {
        yield return new object[] { "FirstDepartmentId", null, null, null };
        yield return new object[] { null, "11", null, null };
        yield return new object[] { null, null, "کامپیوتر", null };
        yield return new object[]
        {
            null, null, null, "FirstFacultyId", DepartmentColumn.FacultyId, true, false,
            null, 0, 25,
            2, false
        };
        yield return new object[]
        {
            null, null, null, null, DepartmentColumn.DepartmentId, true, true,
            (Func<DepartmentSearchDto, IComparable>)(c => c.DepartmentId),
        };
        yield return new object[]
        {
            null, null, null, null, DepartmentColumn.DepartmentCode, true, true,
            (Func<DepartmentSearchDto, IComparable>)(c => c.DepartmentCode),
        };
        yield return new object[]
        {
            null, null, null, null, DepartmentColumn.DepartmentTitle, true, true,
            (Func<DepartmentSearchDto, IComparable>)(c => c.DepartmentTitle),
        };
        yield return new object[]
        {
            null, null, null, null, DepartmentColumn.FacultyId, true, true,
            (Func<DepartmentSearchDto, IComparable>)(c => c.FacultyId),
            0, 25,
            2, false
        };
    }
}