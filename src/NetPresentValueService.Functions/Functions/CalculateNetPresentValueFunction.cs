using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NetPresentValueService.Application.Features.NetPresentValueCalculation;
using NetPresentValueService.Domain.Exceptions;

namespace NetPresentValueService.Functions.Functions
{
    public class CalculateNetPresentValueFunction
    {
        private readonly INetPresentValueService _npvService;
        private readonly ILogger _logger;

        public CalculateNetPresentValueFunction(INetPresentValueService npvService,
            ILogger<CalculateNetPresentValueFunction> logger)
        {
            _npvService = npvService;
            _logger = logger;
        }

        [Function("CalculateNetPresentValue")]
        [EnableCors("localhost:5174")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "npv/calculate")]
            HttpRequestData req)
        {
            try
            {
                var requestBody = await JsonSerializer.DeserializeAsync<NetPresentValueRequestDto>(req.Body);
                if (requestBody == null)
                {
                    return req.CreateResponse(HttpStatusCode.BadRequest);
                }

                var result = await _npvService.CalculateRangeAsync(requestBody);

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(result);
                return response;
            }
            catch (DomainValidationException ex)
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync(ex.Message);
                return badResponse;
            }
            catch (JsonException ex)
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync(ex.Message);
                return badResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}