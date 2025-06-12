using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NetPresentValueService.Application.Features.DiscountRates;

namespace NetPresentValueService.Function.Functions;
public class DiscountRateFunction
{
    private readonly IDiscountRateService _discountService;
    private readonly ILogger _logger;

    public DiscountRateFunction(IDiscountRateService discountService, ILogger<DiscountRateFunction> logger)
    {
        _discountService = discountService;
        _logger = logger;
    }

    [Function("SaveIncrementedDiscountRateDetails")]
    public async Task<HttpResponseData> Save(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "npv/discountDetails")] HttpRequestData req)
    {
        var user = new UserContext(req);
        var dto = await JsonSerializer.DeserializeAsync<IncrementedNetPresentValueDetailsDto>(req.Body);

        if (dto == null || dto.Increment <= 0)
        {
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteStringAsync("Invalid discount rate details.");
            return badResponse;
        }

        await _discountService.SaveIncrementedDiscountRateDetails(user.UserId, dto);
        return req.CreateResponse(HttpStatusCode.NoContent);
    }

    [Function("GetIncrementedDiscountRateDetails")]
    public async Task<HttpResponseData> Load(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "npv/discountDetails")] HttpRequestData req)
    {
        var user = new UserContext(req);
        var dto = await _discountService.LoadIncrementedDiscountRateDetails(user.UserId);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(dto);
        return response;
    }
}

