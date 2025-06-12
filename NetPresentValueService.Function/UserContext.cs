using Microsoft.Azure.Functions.Worker.Http;

namespace NetPresentValueService.Function;

public class UserContext
{
    public string UserId { get; }

    public UserContext(HttpRequestData request)
    {
        var rawHeader = request.Headers.GetValues("x-user-id").FirstOrDefault();
        if (string.IsNullOrWhiteSpace(rawHeader))
        {
            throw new UnauthorizedAccessException("Missing user identifier.");
        }

        UserId = rawHeader;
    }
}
