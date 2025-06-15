using System.Security.Claims;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;

namespace NetPresentValueService.Functions.Test.Helpers;

public class FakeHttpRequestData<T> : HttpRequestData
{
    public FakeHttpRequestData(FunctionContext functionContext, T body) : base(functionContext)
    {
        string request = JsonConvert.SerializeObject(body);
        var bodyStream = new MemoryStream(Encoding.ASCII.GetBytes(request));
        Body = bodyStream;
    }
    public override Stream Body { get; } = new MemoryStream();
    public override HttpHeadersCollection Headers { get; } = new HttpHeadersCollection();
    public override IReadOnlyCollection<IHttpCookie> Cookies { get; }
    public override Uri Url { get; }
    public override IEnumerable<ClaimsIdentity> Identities { get; }
    public override string Method { get; }
    public override HttpResponseData CreateResponse()
    {
        return new FakeHttpResponseData(FunctionContext);
    }
}