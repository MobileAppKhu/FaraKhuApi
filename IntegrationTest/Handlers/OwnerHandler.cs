using System.Net.Http;
using System.Threading.Tasks;
using IntegrationTest.Utilities;
using MarkopTest.Handler;

namespace IntegrationTest.Handlers;

public class OwnerHandler : TestHandler
{
    public override async Task BeforeRequest(HttpClient client)
    {
        await client.Owner();
    }
}