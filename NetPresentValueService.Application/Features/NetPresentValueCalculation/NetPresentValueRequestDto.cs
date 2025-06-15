using NetPresentValueService.Application.Features.DiscountRates;
using NetPresentValueService.Domain.Features.DiscountRates;

namespace NetPresentValueService.Application.Features.NetPresentValueCalculation;

public class NetPresentValueRequestDto
{
    public IncrementedDiscountRateDetailsDto DiscountRateDetails { get; set; }
    public IList<decimal> CashFlows { get; set; }
}