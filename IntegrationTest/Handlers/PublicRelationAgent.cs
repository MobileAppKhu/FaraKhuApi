using System.Net.Http;
using System.Threading.Tasks;
using IntegrationTest.Utilities;
using MarkopTest.Handler;

namespace IntegrationTest.Handlers;

public class PublicRelationOfficerHandler : TestHandler
{
    public override async Task BeforeRequest(HttpClient client)
    {
        await client.PublicRelationAgent();
    }
}