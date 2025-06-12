using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NetPresentValueService.Application.Features.NetPresentValueCalculation;

namespace NetPresentValueService.Function.Functions;

public class CalculateNetPresentValueFunction
{
    private readonly INetPresentValueService _npvService;
    private readonly ILogger _logger;

    public CalculateNetPresentValueFunction(INetPresentValueService npvService, ILogger<CalculateNetPresentValueFunction> logger)
    {
        _npvService = npvService;
        _logger = logger;
    }

    [Function("CalculateNetPresentValue")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "npv/calculate")] HttpRequestData req)
    {
        var user = new UserContext(req);
        var requestBody = await JsonSerializer.DeserializeAsync<List<decimal>>(req.Body);

        if (requestBody == null || requestBody.Count == 0)
        {
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteStringAsync("Cash flow list must not be empty.");
            return badResponse;
        }
        
        var result = await _npvService.CalculateRangeAsync(user.UserId, requestBody);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(result);
        return response;
    }
}
