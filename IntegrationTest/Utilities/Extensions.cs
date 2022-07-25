using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Features.Account.Commands.SignIn;
using Domain.Enum;

namespace IntegrationTest.Utilities;

public static class Extensions
{
    public static async Task<bool> HasErrorCode(this HttpResponseMessage response, ErrorType? errorCode = null)
    {
        var errorProperty = (await response.GetJson())?.EnumerateObject()
            .FirstOrDefault(e => e.Name == "errors").Value;
        var errors = errorProperty?.ValueKind == JsonValueKind.Undefined
            ? null
            : errorProperty?.EnumerateArray().ToArray();
        if (errorCode == null)
            return errors == null || errors.ToArray().Length == 0;
        return errors != null && errors.ToArray().Select(error =>
                (ErrorType) error.EnumerateObject().FirstOrDefault(e => e.Name == "errorType").Value.GetInt32())
            .Any(error => error == errorCode);
    }

    public static async Task<JsonElement?> GetJson(this HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return string.IsNullOrWhiteSpace(content) ? null : JsonSerializer.Deserialize<JsonElement>(content);
    }

    public static async Task<HttpResponseMessage> PostAsync<T>(this HttpClient client, string url, T data)
    {
        return await client.PostAsync(url, JsonContent.Create(data));
    }

    public static async Task<HttpClient> Student(this HttpClient client)
    {
        var data = new SignInCommand
        {
            Logon = "Student@Farakhu.app",
            Password = "StudentPassword"
        };

        var response = await client.PostAsync("/api/Account/SignIn", data);

        foreach (var cookie in response.Headers.GetValues("Set-Cookie").ToArray())
            client.DefaultRequestHeaders.Add("Cookie", cookie);

        return client;
    }

    public static async Task<HttpClient> SecondStudent(this HttpClient client)
    {
        var data = new SignInCommand
        {
            Logon = "SecondStudent@Farakhu.app",
            Password = "SecondStudentPassword"
        };

        var response = await client.PostAsync("/api/Account/SignIn", data);

        foreach (var cookie in response.Headers.GetValues("Set-Cookie").ToArray())
            client.DefaultRequestHeaders.Add("Cookie", cookie);

        return client;
    }

    public static async Task<HttpClient> Instructor(this HttpClient client)
    {
        var data = new SignInCommand
        {
            Logon = "Instructor@Farakhu.app",
            Password = "InstructorPassword"
        };

        var response = await client.PostAsync("api/Account/SignIn", data);

        foreach (var cookie in response.Headers.GetValues("Set-Cookie").ToArray())
            client.DefaultRequestHeaders.Add("Cookie", cookie);

        return client;
    }

    public static async Task<HttpClient> SecondInstructor(this HttpClient client)
    {
        var data = new SignInCommand
        {
            Logon = "SecondInstructor@Farakhu.app",
            Password = "SecondInstructorPassword"
        };

        var response = await client.PostAsync("api/Account/SignIn", data);

        foreach (var cookie in response.Headers.GetValues("Set-Cookie").ToArray())
            client.DefaultRequestHeaders.Add("Cookie", cookie);

        return client;
    }

    public static async Task<HttpClient> Owner(this HttpClient client)
    {
        var data = new SignInCommand
        {
            Logon = "Owner@FaraKhu.app",
            Password = "OwnerPassword"
        };

        var response = await client.PostAsync("api/Account/SignIn", data);

        foreach (var cookie in response.Headers.GetValues("Set-Cookie").ToArray())
            client.DefaultRequestHeaders.Add("Cookie", cookie);

        return client;
    }

    public static async Task<HttpClient> PublicRelationAgent(this HttpClient client)
    {
        var data = new SignInCommand
        {
            Logon = "PublicRelation@FaraKhu.app",
            Password = "PROfficerPassword"
        };

        var response = await client.PostAsync("api/Account/SignIn", data);

        foreach (var cookie in response.Headers.GetValues("Set-Cookie").ToArray())
            client.DefaultRequestHeaders.Add("Cookie", cookie);

        return client;
    }
    
}