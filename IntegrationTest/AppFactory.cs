using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Infrastructure.Persistence;
using IntegrationTest.Utilities;
using MarkopTest.IntegrationTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi;
using Xunit;
using Xunit.Abstractions;
using DatabaseInitializer = IntegrationTest.Persistence.DatabaseInitializer;

namespace IntegrationTest;

public class AppFactory : IntegrationTestFactory<Startup, FetchOptions>
{
    public AppFactory(ITestOutputHelper outputHelper, HttpClient defaultClient)
        : base(outputHelper, defaultClient, new IntegrationTestOptions {HostSeparation = true})
    {
    }

    protected override string GetUrl(string url, string controllerName, string testMethodName)
    {
        return APIs.V1 + url;
    }

    protected override void Initializer(IServiceProvider hostServices)
    {
        new DatabaseInitializer(hostServices).Initialize().GetAwaiter().GetResult();
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d
            => d.ServiceType == typeof(DbContextOptions<DatabaseContext>));

        if (descriptor != null)
            services.Remove(descriptor);

        services.AddDbContextPool<DatabaseContext>(options =>
        {
            options.UseInMemoryDatabase("InMemoryDbForTesting");
        });
    }

    protected override async Task<bool> ValidateResponse(HttpResponseMessage httpResponseMessage,
        FetchOptions fetchOptions)
    {
        var responseHttpStatusCode = fetchOptions.HttpStatusCode;
        if (fetchOptions.ErrorCode != null)
            responseHttpStatusCode = HttpStatusCode.NotAcceptable;
        // if (fetchOptions.ErrorCode == ErrorType.Unauthorized)
        //     responseHttpStatusCode = HttpStatusCode.Unauthorized;

        Assert.Equal(responseHttpStatusCode, httpResponseMessage.StatusCode);

        if (responseHttpStatusCode is HttpStatusCode.OK or HttpStatusCode.NotAcceptable)
            return await httpResponseMessage.HasErrorCode(fetchOptions.ErrorCode);

        return true;
    }
}