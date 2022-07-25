using System.Net;
using Domain.Enum;

namespace IntegrationTest;

public class FetchOptions
{
    public ErrorType? ErrorCode { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;
}