using FluentAssertions;
using NetPresentValueService.Domain.Exceptions;
using NetPresentValueService.Domain.Features.CashFlows;
using NetPresentValueService.Domain.Features.DiscountRates;
using NetPresentValueService.Domain.Features.NetPresentValueCalculation;

namespace NetPresentValueService.Domain.Test.Features.NetPresentValueCalculation;

public class NetPresentValueCalculatorTests
{
    private const decimal DecimalTolerance = 0.001m;
    
    [Fact]
    public void CalculateWithSingleCashFlow()
    {
        // Arrange
        List<decimal> cashFlows = [-1000m];
        var cashFlowSet = new CashFlowSet(cashFlows);
        var discountRate = new DiscountRate(0.05m);
        var expected = -1000m;

        // Act
        var actual = NetPresentValueCalculator.Calculate(cashFlowSet, discountRate);

        // Assert
        expected.Should().BeApproximately(actual.NetPresentValue, DecimalTolerance);
    }

    [Fact]
    public void CalculateWithMultipleCashFlows()
    {
        // Arrange
        List<decimal> cashFlows = [ -1000, 300, 300, 300, 300 ];
        var cashFlowSet = new CashFlowSet(cashFlows);
        var discountRate = new DiscountRate(0.05m);
        var expected = 63.785m;
    
        // Act
        var actual = NetPresentValueCalculator.Calculate(cashFlowSet, discountRate);
    
        // Assert
        expected.Should().BeApproximately(actual.NetPresentValue, DecimalTolerance);
    }
    
    [Fact]
    public void CalculateWithZeroDiscountRate()
    {
        // Arrange
        List<decimal> cashFlows = [ -1000, 300, 300, 300, 300 ];
        var cashFlowSet = new CashFlowSet(cashFlows);
        var discountRate = new DiscountRate(0m);
        var expected = 200m;
    
        // Act
        var actual = NetPresentValueCalculator.Calculate(cashFlowSet, discountRate);
    
        // Assert
        expected.Should().BeApproximately(actual.NetPresentValue, DecimalTolerance);
    }
    
    [Fact]
    public void CalculateWithNegativeDiscountRate()
    {
        // Arrange
        List<decimal> cashFlows = [ -1000, 1100 ];
        var cashFlowSet = new CashFlowSet(cashFlows);
        var discountRate = new DiscountRate(-0.05m);
        var expected = 157.895m;
    
        // Act
        var actual = NetPresentValueCalculator.Calculate(cashFlowSet, discountRate);
    
        // Assert
        expected.Should().BeApproximately(actual.NetPresentValue, DecimalTolerance);
    }
    
    [Fact]
    public void CalculateWhenNetPresentValueOverflowsDecimalMaxValue()
    {
        // Arrange
        List<decimal> cashFlows = [ decimal.MaxValue, 1 ];
        var cashFlowSet = new CashFlowSet(cashFlows);
        var discountRate = new DiscountRate(0.05m);
    
        // Act
        var act = () => NetPresentValueCalculator.Calculate(cashFlowSet, discountRate);
    
        // Assert
        act.Should().Throw<DomainValidationException>()
            .WithMessage($"The sum of all discounted cash flows for any given discount rate should always lie between {decimal.MinValue} and {decimal.MaxValue}.");
    }
    
    [Fact]
    public void CalculateWhenNetPresentValueOverflowsDecimalMinValue()
    {
        // Arrange
        List<decimal> cashFlows = [ decimal.MinValue, -1 ];
        var cashFlowSet = new CashFlowSet(cashFlows);
        var discountRate = new DiscountRate(0.05m);
    
        // Act
        var act = () => NetPresentValueCalculator.Calculate(cashFlowSet, discountRate);
    
        // Assert
        act.Should().Throw<DomainValidationException>()
            .WithMessage($"The sum of all discounted cash flows for any given discount rate should always lie between {decimal.MinValue} and {decimal.MaxValue}.");
    }
}