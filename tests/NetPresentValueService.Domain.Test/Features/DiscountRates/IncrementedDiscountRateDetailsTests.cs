using FluentAssertions;
using NetPresentValueService.Domain.Features.DiscountRates;

namespace NetPresentValueService.Domain.Test.Features.DiscountRates;

public class IncrementedDiscountRateDetailsTests
{
    [Fact]
    public void CreateValidIncrementedDiscountRateDetails()
    {
        //Assert
        var lowerBound = new DiscountRate(0.1m);
        var upperBound = new DiscountRate(0.6m);
        var increment = new DiscountRate(0.015m);
        
        //Act
        var actual = new IncrementedDiscountRateDetails(lowerBound, upperBound, increment);
        
        //Assert
        actual.Increment.Value.Should().Be(increment.Value);
        actual.LowerBoundDiscountRate.Value.Should().Be(lowerBound.Value);
        actual.UpperBoundDiscountRate.Value.Should().Be(upperBound.Value);
    }
    
    [Fact]
    public void LowerBoundHigherThanUpperBound()
    {
        //Assert
        var lowerBound = new DiscountRate(0.2m);
        var upperBound = new DiscountRate(0.1m);
        var increment = new DiscountRate(0.015m);
        
        //Act
        var act = () => new IncrementedDiscountRateDetails(lowerBound, upperBound, increment);
        
        //Assert
        act.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void IncrementIsNegative()
    {
        //Assert
        var lowerBound = new DiscountRate(0.1m);
        var upperBound = new DiscountRate(0.2m);
        var increment = new DiscountRate(-0.015m);
        
        //Act
        var act = () => new IncrementedDiscountRateDetails(lowerBound, upperBound, increment);
        
        //Assert
        act.Should().Throw<ArgumentException>();
    }
        
    [Fact]
    public void IncrementIsZero()
    {
        //Assert
        var lowerBound = new DiscountRate(0.1m);
        var upperBound = new DiscountRate(0.2m);
        var increment = new DiscountRate(0m);
        
        //Act
        var act = () => new IncrementedDiscountRateDetails(lowerBound, upperBound, increment);
        
        //Assert
        act.Should().Throw<ArgumentException>();
    }
}