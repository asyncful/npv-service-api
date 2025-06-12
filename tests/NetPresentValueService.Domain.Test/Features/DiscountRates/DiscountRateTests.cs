using FluentAssertions;
using NetPresentValueService.Domain.Features.DiscountRates;
using Xunit;

namespace NetPresentValueService.Domain.Test.Features.DiscountRates;

public class DiscountRateTests
{
    [Fact]
    public void ValueIsTooLow()
    {
        //Act
        var act = () => new DiscountRate(-2);
        
        //Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
    
    [Fact]
    public void ValueIsTooHigh()
    {
        //Act
        var act = () => new DiscountRate(2);
        
        //Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}