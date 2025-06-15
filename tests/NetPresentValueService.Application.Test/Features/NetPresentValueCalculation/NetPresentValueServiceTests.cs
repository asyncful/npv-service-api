using FluentAssertions;
using Moq;
using NetPresentValueService.Application.Features.DiscountRates;
using NetPresentValueService.Application.Features.NetPresentValueCalculation;
using NetPresentValueService.Domain.Features.DiscountRates;

namespace NetPresentValueService.Application.Test.Features.NetPresentValueCalculation;

public class NetPresentValueServiceTests
{
    private readonly Application.Features.NetPresentValueCalculation.NetPresentValueService _sut = new();

    private const decimal DecimalTolerance = 0.0001m;

    [Fact]
    public async Task CalculateRangeAsyncWithValidInput()
    {
        // Arrange
        var rateDetails = new IncrementedDiscountRateDetailsDto
        {
            LowerBoundDiscountRate = 0.05m,
            UpperBoundDiscountRate = 0.10m,
            Increment =  0.05m,
        };

        var cashFlows = new List<decimal> { 100, 200, 300 };
        var requestDto = new NetPresentValueRequestDto
        {
            CashFlows = cashFlows,
            DiscountRateDetails = rateDetails
        };
        
        var expectedResults = new List<NetPresentValueResultDto>
        {
            new (){ DiscountRate = 0.05m, NetPresentValue = 562.585m},
            new (){ DiscountRate = 0.10m, NetPresentValue = 529.752m}
        };
        

        // Act
        var actualResults = (await _sut.CalculateRangeAsync(requestDto)).Results;

        // Assert
        for (var i = 0; i < actualResults.Count; i++)
        {
            var dto = actualResults[i];
            dto.NetPresentValue.Should().BeApproximately(expectedResults[i].NetPresentValue, DecimalTolerance);
            dto.DiscountRate.Should().BeApproximately(expectedResults[i].DiscountRate, DecimalTolerance);
        }
    }
 }