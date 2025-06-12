using Microsoft.Azure.Functions.Worker.Http;
using Moq;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;

namespace NetPresentValueService.Function.Test.Functions;

public static class HttpTestHelper
{
    public static HttpRequestData CreateHttpRequest<T>(FunctionContext context, T body)
    {
        var json = JsonSerializer.Serialize(body);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var request = new Mock<HttpRequestData>(context);
        request.Setup(r => r.Body).Returns(stream);
        return request.Object;
    }

    public static HttpResponseData CreateHttpResponse(FunctionContext context, HttpStatusCode statusCode, string? content = null)
    {
        var response = new Mock<HttpResponseData>(context);
        response.SetupProperty(r => r.StatusCode, statusCode);
        response.Setup(r => r.WriteStringAsync(It.IsAny<string>(), default)).Returns(Task.CompletedTask);
        return response.Object;
    }
}
