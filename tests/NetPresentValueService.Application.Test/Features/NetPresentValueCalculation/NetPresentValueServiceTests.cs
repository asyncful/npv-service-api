using FluentAssertions;
using Moq;
using NetPresentValueService.Application.Features.NetPresentValueCalculation;
using NetPresentValueService.Domain.Features.DiscountRates;

namespace NetPresentValueService.Application.Test.Features.NetPresentValueCalculation;

public class NetPresentValueServiceTests
{
    private readonly Mock<IDiscountRateRepository> _discountRateRepositoryMock = new ();
    private readonly Application.Features.NetPresentValueCalculation.NetPresentValueService _sut;

    private const decimal DecimalTolerance = 0.0001m;

    public NetPresentValueServiceTests()
    {
        _sut = new Application.Features.NetPresentValueCalculation.NetPresentValueService(_discountRateRepositoryMock.Object);
    }

    [Fact]
    public async Task CalculateRangeAsyncWithValidInput_ReturnsCorrectResults()
    {
        // Arrange
        var userId = "user123";
        var details = new IncrementedDiscountRateDetails(new (0.05m), new (0.10m), new (0.05m));
        _discountRateRepositoryMock.Setup(r => r.GetAsync(userId)).ReturnsAsync(details);

        var cashFlows = new List<decimal> { 100, 200, 300 };
        var expectedDtos = new List<NetPresentValueResultDto>
        {
            new (){ DiscountRate = 0.05m, NetPresentValue = 562.585m},
            new (){ DiscountRate = 0.10m, NetPresentValue = 529.752m}
        };

        // Act
        var actualDtos = (await _sut.CalculateRangeAsync(userId, cashFlows)).ToList();

        // Assert
        for (var i = 0; i < actualDtos.Count; i++)
        {
            var dto = actualDtos[i];
            dto.NetPresentValue.Should().BeApproximately(expectedDtos[i].NetPresentValue, DecimalTolerance);
            dto.DiscountRate.Should().BeApproximately(expectedDtos[i].DiscountRate, DecimalTolerance);
        }
    }

    [Fact]
    public async Task CalculateRangeAsyncWithEmptyCashFlows_ReturnsZeroNpv()
    {
        // Arrange
        var userId = "user123";
        var discountSettings = new IncrementedDiscountRateDetails(new(0.05m), new(0.05m), new(0.01m));
        _discountRateRepositoryMock
            .Setup(r => r.GetAsync(userId))
            .ReturnsAsync(discountSettings);

        var cashFlows = new List<decimal>();

        // Act
        var results = await _sut.CalculateRangeAsync(userId, cashFlows);

        // Assert
        var result = results.Single();
        result.DiscountRate.Should().BeApproximately(0.05m, DecimalTolerance);
        result.NetPresentValue.Should().BeApproximately(0.00m, DecimalTolerance);
    }

    [Fact]
    public async Task CalculateRangeAsync_CallsRepositoryExactlyOnce()
    {
        // Arrange
        var userId = "user123";
        var settings = new IncrementedDiscountRateDetails(new(0.01m), new(0.01m), new(0.01m));
        _discountRateRepositoryMock
            .Setup(r => r.GetAsync(userId))
            .ReturnsAsync(settings);

        var cashFlows = new List<decimal> { 100 };

        // Act
        await _sut.CalculateRangeAsync(userId, cashFlows);

        // Assert
        _discountRateRepositoryMock.Verify(r => r.GetAsync(userId), Times.Once);
    }
}