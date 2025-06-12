using FluentAssertions;
using NetPresentValueService.Domain.Features.DiscountRates;
using NetPresentValueService.Domain.Features.NetPresentValueCalculation;
using Xunit;

namespace NetPresentValueService.Domain.Test.Features.NetPresentValueCalculation;

public class NetPresentValueCalculatorTests
{
    [Fact]
    public void SingleCashFlowTest()
    {
        // Arrange
        List<decimal> cashFlows = [-1000m];
        var discountRate = new DiscountRate(0.05m);
        var expected = -1000m;

        // Act
        var actual = NetPresentValueCalculator.Calculate(cashFlows, discountRate);

        // Assert
        expected.Should().BeApproximately(actual, 0.001m);
    }

    [Fact]
    public void MultipleCashFlowsTest()
    {
        // Arrange
        List<decimal> cashFlows = [ -1000, 300, 300, 300, 300 ];
        var discountRate = new DiscountRate(0.05m);
        var expected = 63.785m;

        // Act
        var actual = NetPresentValueCalculator.Calculate(cashFlows, discountRate);

        // Assert
        expected.Should().BeApproximately(actual, 0.001m);
    }

    [Fact]
    public void EmptyCashFlowsTest()
    {
        // Arrange
        List<decimal> cashFlows = [ ];
        var discountRate = new DiscountRate(0.05m);
        var expected = 0m;

        // Act
        var actual = NetPresentValueCalculator.Calculate(cashFlows, discountRate);

        // Assert
        expected.Should().BeApproximately(actual, 0.001m);
    }

    [Fact]
    public void ZeroDiscountRateTest()
    {
        // Arrange
        List<decimal> cashFlows = [ -1000, 300, 300, 300, 300 ];
        var discountRate = new DiscountRate(0m);
        var expected = 200m;

        // Act
        var actual = NetPresentValueCalculator.Calculate(cashFlows, discountRate);

        // Assert
        expected.Should().BeApproximately(actual, 0.001m);
    }

    [Fact]
    public void NegativeDiscountRateTest()
    {
        // Arrange
        List<decimal> cashFlows = [ -1000, 1100 ];
        var discountRate = new DiscountRate(-0.05m);
        var expected = 157.895m;

        // Act
        var actual = NetPresentValueCalculator.Calculate(cashFlows, discountRate);

        // Assert
        expected.Should().BeApproximately(actual, 0.001m);
    }

    [Fact]
    public void NullCashFlowsTest()
    {
        // Arrange
        List<decimal> cashFlows = null;
        var discountRate = new DiscountRate(0.05m);

        // Act
        var act = () => NetPresentValueCalculator.Calculate(cashFlows, discountRate);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void NetPresentValueOverflowsDecimalMaxValue()
    {
        // Arrange
        List<decimal> cashFlows = [ decimal.MaxValue, 1 ];
        var discountRate = new DiscountRate(0.05m);

        // Act
        var act = () => NetPresentValueCalculator.Calculate(cashFlows, discountRate);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void NetPresentValueOverflowsDecimalMinValue()
    {
        // Arrange
        List<decimal> cashFlows = [ decimal.MinValue, -1 ];
        var discountRate = new DiscountRate(0.05m);

        // Act
        var act = () => NetPresentValueCalculator.Calculate(cashFlows, discountRate);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}