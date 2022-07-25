using System.Net.Http;
using System.Threading.Tasks;
using IntegrationTest.Utilities;
using MarkopTest.Handler;

namespace IntegrationTest.Handlers;

public class StudentHandler : TestHandler
{
    public override async Task BeforeRequest(HttpClient client)
    {
        await client.Student();
    }
}