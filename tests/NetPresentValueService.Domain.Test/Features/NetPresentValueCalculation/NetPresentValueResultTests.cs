using FluentAssertions;
using NetPresentValueService.Domain.Features.DiscountRates;
using NetPresentValueService.Domain.Features.NetPresentValueCalculation;

namespace NetPresentValueService.Domain.Test.Features.NetPresentValueCalculation;

public class NetPresentValueResultTests
{
    private const decimal DecimalTolerance = 0.001m;
    
    [Fact]
    public void InstantiateWithNullDiscountRate()
    {
        //Arrange
        var discountRate = default(DiscountRate);
        
        //Act
        var act = () => new NetPresentValueResult(discountRate, 0m);
        
        //Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("discountRate");
    }
    
    [Fact]
    public void InstantiateValidParameters()
    {
        //Arrange
        var discountRate = 1m;
        var npv = 0m;
        
        //Act
        var actual = new NetPresentValueResult(new DiscountRate(discountRate), npv);
        
        //Assert
        actual.DiscountRate.Value.Should().BeApproximately(discountRate, DecimalTolerance);
        actual.NetPresentValue.Should().BeApproximately(npv, DecimalTolerance);
    }
}