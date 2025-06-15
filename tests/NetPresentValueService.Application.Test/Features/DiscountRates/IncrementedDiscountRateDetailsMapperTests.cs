using FluentAssertions;
using NetPresentValueService.Application.Features.DiscountRates;
using NetPresentValueService.Domain.Features.DiscountRates;

namespace NetPresentValueService.Application.Test.Features.DiscountRates;

public class IncrementedDiscountRateDetailsMapperTests
{
    private const decimal DecimalTolerance = 0.001m;
    
    [Fact]
    public void MapToDto()
    {
        //Arrange
        var lowerBoundRate = 0.1m;
        var upperBoundRate = 0.2m;
        var increment = 0.01m;
        var dto = new IncrementedDiscountRateDetailsDto
        {
            Increment = increment,
            LowerBoundDiscountRate = lowerBoundRate,
            UpperBoundDiscountRate = upperBoundRate,
        };
        
        //Act
        var actualDomainModel = dto.ToDomainModel();
        
        //Assert
        actualDomainModel.Increment.Value.Should().BeApproximately(increment, DecimalTolerance);
        actualDomainModel.LowerBoundDiscountRate.Value.Should().BeApproximately(lowerBoundRate, DecimalTolerance);
        actualDomainModel.UpperBoundDiscountRate.Value.Should().BeApproximately(upperBoundRate, DecimalTolerance);
    }
}