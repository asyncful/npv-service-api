using NetPresentValueService.Application.Features.DiscountRates;
using NetPresentValueService.Domain.Features.DiscountRates;
using Moq;

namespace NetPresentValueService.Application.Test.Features.DiscountRates;

public class DiscountRateServiceTests
{
    private readonly Mock<IDiscountRateRepository> _repoMock = new ();
    private readonly DiscountRateService _service;

    public DiscountRateServiceTests()
    {
        _service = new DiscountRateService(_repoMock.Object);
    }

    [Fact]
    public async Task SaveIncrementedDiscountRateDetails_ValidDto_SavesToRepository()
    {
        // Arrange
        var userId = "user123";
        var dto = new IncrementedNetPresentValueDetailsDto
        {
            LowerBoundDiscountRate = 0.01m,
            UpperBoundDiscountRate = 0.05m,
            Increment = 0.01m
        };

        // Act
        await _service.SaveIncrementedDiscountRateDetails(userId, dto);

        // Assert
        _repoMock.Verify(r => r.SaveAsync(userId, It.Is<IncrementedDiscountRateDetails>(
            d => d.LowerBoundDiscountRate.Value == dto.LowerBoundDiscountRate &&
                 d.UpperBoundDiscountRate.Value == dto.UpperBoundDiscountRate &&
                 d.Increment.Value == dto.Increment
        )), Times.Once);
    }

    [Fact]
    public async Task LoadIncrementedDiscountRateDetails_ReturnsExpectedDto()
    {
        // Arrange
        var userId = "user123";
        var domainDetails = new IncrementedDiscountRateDetails(
            new DiscountRate(0.01m),
            new DiscountRate(0.05m),
            new DiscountRate(0.01m)
        );

        _repoMock.Setup(r => r.GetAsync(userId)).ReturnsAsync(domainDetails);

        // Act
        var result = await _service.LoadIncrementedDiscountRateDetails(userId);

        // Assert
        Assert.Equal(0.01m, result.LowerBoundDiscountRate);
        Assert.Equal(0.05m, result.UpperBoundDiscountRate);
        Assert.Equal(0.01m, result.Increment);
    }
}