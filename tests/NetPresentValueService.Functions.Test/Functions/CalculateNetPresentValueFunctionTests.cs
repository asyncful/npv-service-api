using System.Net;
using Azure.Core.Serialization;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NetPresentValueService.Application.Features.DiscountRates;
using NetPresentValueService.Application.Features.NetPresentValueCalculation;
using NetPresentValueService.Domain.Exceptions;
using NetPresentValueService.Functions.Functions;
using NetPresentValueService.Functions.Test.Helpers;

namespace NetPresentValueService.Functions.Test.Functions;

public class CalculateNetPresentValueFunctionTests
{
    private readonly Mock<INetPresentValueService> _npvServiceMock = new();
    private readonly Mock<ILogger<CalculateNetPresentValueFunction>> _loggerMock = new();
    private readonly Mock<FunctionContext> _context = new ();
    private readonly Mock<IServiceProvider> _internalServices = new ();

    public CalculateNetPresentValueFunctionTests()
    {
        var service = new Mock<IOptions<WorkerOptions>>();
        var serializer = new JsonObjectSerializer();
        var workerOptions = new WorkerOptions()
        {
            Serializer = serializer,
        };
        service.Setup(x => x.Value).Returns(workerOptions);
        _internalServices.Setup(p => p.GetService(It.IsAny<Type>())).Returns(service.Object);
        _context.Setup(x => x.InstanceServices).Returns(_internalServices.Object);
    }

    [Fact]
    public async Task RunWithValidRequestDto()
    {
        // Arrange
        var requestDto = new NetPresentValueRequestDto
        {
            CashFlows =  new List<decimal> { 100, 200, 300 },
            DiscountRateDetails = new IncrementedDiscountRateDetailsDto
            {
                Increment = 0.5m,
                LowerBoundDiscountRate = 0m,
                UpperBoundDiscountRate = 1m,
            }
        };
        
        var responseDto = new NetPresentValueResultSetDto
        {
            Results = [new() { DiscountRate = 0.05m, NetPresentValue = 550.0m }]
        };

        _npvServiceMock
            .Setup(s => s.CalculateRangeAsync(It.IsAny<NetPresentValueRequestDto>()))
            .ReturnsAsync(responseDto);

        var requestData = new FakeHttpRequestData<NetPresentValueRequestDto>(_context.Object, requestDto);
        var func = new CalculateNetPresentValueFunction(_npvServiceMock.Object, _loggerMock.Object);

        // Act
        var response = await func.Run(requestData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RunWithInvalidRequestDtoStructure()
    {
        //Arrange
        var requestData = new FakeHttpRequestData<string>(_context.Object, "hello");
        var func = new CalculateNetPresentValueFunction(_npvServiceMock.Object, _loggerMock.Object);
    
        //Act
        var response = await func.Run(requestData);
    
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task RunWithInvalidDomainValues()
    {
        // Arrange
        var requestDto = new NetPresentValueRequestDto
        {
            CashFlows =  new List<decimal> { 100, 200, 300 },
            DiscountRateDetails = new IncrementedDiscountRateDetailsDto
            {
                Increment = 5m,
                LowerBoundDiscountRate = 3m,
                UpperBoundDiscountRate = 3m,
            }
        };

        _npvServiceMock
            .Setup(s => s.CalculateRangeAsync(It.IsAny<NetPresentValueRequestDto>()))
            .Throws(new DomainValidationException(""));

        var requestData = new FakeHttpRequestData<NetPresentValueRequestDto>(_context.Object, requestDto);
        var func = new CalculateNetPresentValueFunction(_npvServiceMock.Object, _loggerMock.Object);

        // Act
        var response = await func.Run(requestData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task RunWhenUnexpectedExceptionOccurs()
    {
        // Arrange
        var requestDto = new NetPresentValueRequestDto
        {
            CashFlows =  new List<decimal> { 100, 200, 300 },
            DiscountRateDetails = new IncrementedDiscountRateDetailsDto
            {
                Increment = 5m,
                LowerBoundDiscountRate = 3m,
                UpperBoundDiscountRate = 3m,
            }
        };

        _npvServiceMock
            .Setup(s => s.CalculateRangeAsync(It.IsAny<NetPresentValueRequestDto>()))
            .Throws(new Exception(""));

        var requestData = new FakeHttpRequestData<NetPresentValueRequestDto>(_context.Object, requestDto);
        var func = new CalculateNetPresentValueFunction(_npvServiceMock.Object, _loggerMock.Object);

        // Act
        var response = await func.Run(requestData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}