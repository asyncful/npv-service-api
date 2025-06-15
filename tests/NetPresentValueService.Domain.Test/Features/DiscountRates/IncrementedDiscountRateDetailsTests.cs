using FluentAssertions;
using NetPresentValueService.Domain.Exceptions;
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
    public void InstantiateWithLowerBoundHigherThanUpperBound()
    {
        //Assert
        var lowerBound = new DiscountRate(0.2m);
        var upperBound = new DiscountRate(0.1m);
        var increment = new DiscountRate(0.015m);
        
        //Act
        var act = () => new IncrementedDiscountRateDetails(lowerBound, upperBound, increment);
        
        //Assert
        act.Should().Throw<DomainValidationException>().WithMessage("Upper bound discount rate must be greater than or equal to lower bound discount rate.");
    }
    
    [Fact]
    public void InstantiateWithNegativeIncrement()
    {
        //Assert
        var lowerBound = new DiscountRate(0.1m);
        var upperBound = new DiscountRate(0.2m);
        var increment = new DiscountRate(-0.015m);
        
        //Act
        var act = () => new IncrementedDiscountRateDetails(lowerBound, upperBound, increment);
        
        //Assert
        act.Should().Throw<DomainValidationException>().WithMessage("Increment must be positive.");
    }
        
    [Fact]
    public void InstantiateWithZeroIncrement()
    {
        //Assert
        var lowerBound = new DiscountRate(0.1m);
        var upperBound = new DiscountRate(0.2m);
        var increment = new DiscountRate(0m);
        
        //Act
        var act = () => new IncrementedDiscountRateDetails(lowerBound, upperBound, increment);
        
        //Assert
        act.Should().Throw<DomainValidationException>().WithMessage("Increment must be positive.");
    }
    
            
    [Fact]
    public void InstantiateWithValuesThatWouldGenerateOver100DiscountRates()
    {
        //Assert
        var lowerBound = new DiscountRate(0m);
        var upperBound = new DiscountRate(1m);
        var increment = new DiscountRate(0.0001m);
        
        //Act
        var act = () => new IncrementedDiscountRateDetails(lowerBound, upperBound, increment);
        
        //Assert
        act.Should().Throw<DomainValidationException>().WithMessage("Upper and Lower bound discount rates should not allow for more than 100 incremented discount rates.");
    }
}