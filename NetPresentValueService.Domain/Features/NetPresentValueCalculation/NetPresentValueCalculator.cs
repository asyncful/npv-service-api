using NetPresentValueService.Domain.Exceptions;
using NetPresentValueService.Domain.Features.CashFlows;
using NetPresentValueService.Domain.Features.DiscountRates;

namespace NetPresentValueService.Domain.Features.NetPresentValueCalculation;

public static class NetPresentValueCalculator
{
    public static NetPresentValueResult Calculate(CashFlowSet cashFlows, DiscountRate discountRate)
    {
        var npv = 0m;
        var currentDiscountRate = 1m;
        try
        {
            foreach (var cashFlow in cashFlows.Values)
            {
                npv += cashFlow / currentDiscountRate;
                
                currentDiscountRate *= 1 + discountRate.Value;
            }
        }
        catch (OverflowException)
        {
            throw new DomainValidationException($"The sum of all discounted cash flows for any given discount rate should always lie between {decimal.MinValue} and {decimal.MaxValue}.");
        }

        return new (discountRate, npv);
    }
}