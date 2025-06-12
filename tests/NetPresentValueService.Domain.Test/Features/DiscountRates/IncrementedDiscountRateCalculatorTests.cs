using FluentAssertions;
using NetPresentValueService.Domain.Features.DiscountRates;

namespace NetPresentValueService.Domain.Test.Features.DiscountRates;

public class IncrementedDiscountRateCalculatorTests
{
    private const decimal DecimalTolerance = 0.00001m;
    
    [Fact]
    public void LowerBoundIsEqualToUpperBound()
    {
        //Assert
        var lowerBound = new DiscountRate(0.2m);
        var upperBound = new DiscountRate(0.2m);
        var increment = new DiscountRate(0.1m);
        var details = new IncrementedDiscountRateDetails(lowerBound, upperBound, increment);
        
        List<DiscountRate> expectedRates = [lowerBound];

        //Act
        var actualRates = IncrementedDiscountRateCalculator.Calculate(details).ToList();

        //Assert
        for (var i = 0; i < actualRates.Count; i++)
        {
            actualRates[i].Value.Should().BeApproximately(expectedRates[i].Value, DecimalTolerance);
        }
    }
    
    [Fact]
    public void IncrementIsHalfTheDifferenceBetweenUpperAndLowerBound()
    {
        //Assert
        var lowerBound = new DiscountRate(0.1m);
        var upperBound = new DiscountRate(0.15m);
        var increment = new DiscountRate(0.025m);
        var details = new IncrementedDiscountRateDetails(lowerBound, upperBound, increment);
        
        var middleRate =  new DiscountRate(lowerBound.Value + increment.Value);
        List<DiscountRate> expectedRates = [lowerBound, middleRate, upperBound];

        //Act
        var actualRates = IncrementedDiscountRateCalculator.Calculate(details).ToList();

        //Assert
        for (var i = 0; i < actualRates.Count; i++)
        {
            actualRates[i].Value.Should().BeApproximately(expectedRates[i].Value, DecimalTolerance);
        }
    }
        
    [Fact]
    public void IncrementIsOverHalfTheDifferenceBetweenUpperAndLowerBound()
    {
        //Assert
        var lowerBound = new DiscountRate(0.1m);
        var upperBound = new DiscountRate(0.15m);
        var increment = new DiscountRate(0.03m);
        var details = new IncrementedDiscountRateDetails(lowerBound, upperBound, increment);
        
        var middleRate =  new DiscountRate(lowerBound.Value + increment.Value);
        List<DiscountRate> expectedRates = [lowerBound, middleRate];

        //Act
        var actualRates = IncrementedDiscountRateCalculator.Calculate(details);

        //Assert
        expectedRates.Should().BeEquivalentTo(actualRates);
    }
    
            
    [Fact]
    public void SmallIncrementGeneratesManyDiscountRates()
    {
        //Assert
        var lowerBound = new DiscountRate(0.1m);
        var upperBound = new DiscountRate(0.2m);
        var increment = new DiscountRate(0.001m);
        var details = new IncrementedDiscountRateDetails(lowerBound, upperBound, increment);
        var expectedRateCount = 101;

        //Act
        var actualRates = IncrementedDiscountRateCalculator.Calculate(details).ToList();

        //Assert
        actualRates.Count.Should().Be(expectedRateCount);
        actualRates.First().Value.Should().BeApproximately(lowerBound.Value, DecimalTolerance);
        actualRates.Last().Value.Should().BeApproximately(upperBound.Value, DecimalTolerance);
        
        var previousRate = actualRates.First();
        foreach (var rate in actualRates.Skip(1))
        {
            rate.Value.Should().BeApproximately(previousRate.Value + increment.Value, DecimalTolerance);
            previousRate = rate;
        }
    }

}