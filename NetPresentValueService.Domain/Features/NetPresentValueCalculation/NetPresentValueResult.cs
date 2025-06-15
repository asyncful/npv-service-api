using NetPresentValueService.Domain.Exceptions;
using NetPresentValueService.Domain.Features.DiscountRates;

namespace NetPresentValueService.Domain.Features.NetPresentValueCalculation;

public class NetPresentValueResult
{
    public DiscountRate DiscountRate { get; }
    public decimal NetPresentValue { get; }

    public NetPresentValueResult(DiscountRate discountRate, decimal netPresentValue)
    {
        ArgumentNullException.ThrowIfNull(discountRate);
        
        DiscountRate = discountRate;
        NetPresentValue = netPresentValue;
    }
}