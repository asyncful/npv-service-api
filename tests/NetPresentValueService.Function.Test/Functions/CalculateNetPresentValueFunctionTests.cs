using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using NetPresentValueService.Application.Features.NetPresentValueCalculation;
using NetPresentValueService.Function.Functions;

namespace NetPresentValueService.Function.Test.Functions;

public class CalculateNetPresentValueFunctionTests
{
    private readonly Mock<INetPresentValueService> _npvServiceMock = new();
    private readonly Mock<ILogger<CalculateNetPresentValueFunction>> _loggerMock = new();
    private readonly FunctionContext _context = Mock.Of<FunctionContext>();

    [Fact]
    public async Task Run_WithValidCashFlows_ReturnsOkResponse()
    {
        // Arrange
        var userId = "user123";
        var input = new List<decimal> { 100, 200, 300 };
        var expected = new List<NetPresentValueResultDto>
        {
            new() { DiscountRate = 0.05m, NetPresentValue = 550.0m }
        };

        _npvServiceMock.Setup(s => s.CalculateRangeAsync(userId, input)).ReturnsAsync(expected);

        var request = HttpTestHelper.CreateHttpRequest(_context, input);
        var func = new CalculateNetPresentValueFunction(_npvServiceMock.Object, _loggerMock.Object);

        // Act
        var response = await func.Run(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Run_WithEmptyInput_ReturnsBadRequest()
    {
        var request = HttpTestHelper.CreateHttpRequest(_context, new List<decimal>());
        var func = new CalculateNetPresentValueFunction(_npvServiceMock.Object, _loggerMock.Object);

        var response = await func.Run(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}