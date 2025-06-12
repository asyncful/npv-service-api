namespace NetPresentValueService.Domain.Features.DiscountRates;

public static class IncrementedDiscountRateCalculator
{
    public static IEnumerable<DiscountRate> Calculate(IncrementedDiscountRateDetails details)
    {
        var currentRate = details.LowerBoundDiscountRate.Value;
        while (currentRate <= details.UpperBoundDiscountRate.Value)
        {
            yield return new DiscountRate(currentRate);
            
            currentRate += details.Increment.Value;
        }
    }
}