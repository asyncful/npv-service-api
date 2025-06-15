using FluentAssertions;
using NetPresentValueService.Domain.Exceptions;
using NetPresentValueService.Domain.Features.DiscountRates;
using Xunit;

namespace NetPresentValueService.Domain.Test.Features.DiscountRates;

public class DiscountRateTests
{
    [Fact]
    public void CreateDiscountRateWithValueTooLow()
    {
        //Act
        var act = () => new DiscountRate(-2);
        
        //Assert
        act.Should().Throw<DomainValidationException>().WithMessage("A discount rate should be between -1.0 and 1.0.");
    }
    
    [Fact]
    public void CreateDiscountRateWithValueTooHigh()
    {
        //Act
        var act = () => new DiscountRate(2);
        
        //Assert
        act.Should().Throw<DomainValidationException>().WithMessage("A discount rate should be between -1.0 and 1.0.");
    }
        
    [Fact]
    public void CreateValidDiscountRate()
    {
        //Arrange
        var value = 0.5m;
        
        //Act
        var actual = new DiscountRate(value);
        
        //Assert
        actual.Value.Should().BeApproximately(value, 0.001m);
    }
}