using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NetPresentValueService.Application.Features.NetPresentValueCalculation;
using NetPresentValueService.Domain.Exceptions;

namespace NetPresentValueService.Functions.Functions
{
    public class CalculateNetPresentValueFunction
    {
        private const string RoutePath = "npv/calculate";
        
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
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = RoutePath)]
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
                return await CreateProblemResponse(req, HttpStatusCode.BadRequest, "Validation error", ex.Message); ;
            }
            catch (JsonException ex)
            {
                return await CreateProblemResponse(req, HttpStatusCode.BadRequest, "Json parsing error", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return await CreateProblemResponse(req, HttpStatusCode.InternalServerError, "Unexpected error", "Unexpected error occurred");
            }
        }
        
        private static async Task<HttpResponseData> CreateProblemResponse(
            HttpRequestData req,
            HttpStatusCode statusCode,
            string title,
            string detail)
        {
            var problem = new ProblemDetails
            {
                Title = title,
                Status = (int)statusCode,
                Detail = detail,
                Instance = RoutePath
            };

            var response = req.CreateResponse(statusCode);
            await response.WriteAsJsonAsync(problem);
            return response;
        }
    }
}