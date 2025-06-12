using NetPresentValueService.Domain.Features.DiscountRates;

namespace NetPresentValueService.Domain.Features.NetPresentValueCalculation;

public static class NetPresentValueCalculator
{
    public static decimal Calculate(List<decimal> cashFlows, DiscountRate discountRate)
    {
        ArgumentNullException.ThrowIfNull(cashFlows);
        var npv = 0m;
        var currentDiscountRate = 1m;
        try
        {
            foreach (var cashFlow in cashFlows)
            {
                npv += cashFlow / currentDiscountRate;
                
                currentDiscountRate *= 1 + discountRate.Value;
            }
        }
        catch (OverflowException)
        {
            throw new ArgumentException($"The sum of discounted cash flows reached the maximum limit {decimal.MaxValue} or minimum limit {decimal.MinValue}.");
        }

        return npv;
    }
}