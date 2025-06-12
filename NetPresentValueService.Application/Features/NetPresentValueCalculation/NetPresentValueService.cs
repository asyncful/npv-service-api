using NetPresentValueService.Domain.Features.DiscountRates;
using NetPresentValueService.Domain.Features.NetPresentValueCalculation;

namespace NetPresentValueService.Application.Features.NetPresentValueCalculation;

public class NetPresentValueService : INetPresentValueService
{
    private readonly IDiscountRateRepository _discountRateRepository;

    public NetPresentValueService(IDiscountRateRepository discountRateRepository)
    {
        _discountRateRepository = discountRateRepository;
    }

    public async Task<IEnumerable<NetPresentValueResultDto>> CalculateRangeAsync(string userId, List<decimal> cashFlows)
    {
        var details = await _discountRateRepository.GetAsync(userId);

        var results = new List<NetPresentValueResultDto>();

        var discountRates = IncrementedDiscountRateCalculator.Calculate(details);
        foreach (var discountRate in discountRates)
        {
            var npv = NetPresentValueCalculator.Calculate(cashFlows, discountRate);
            var resultDto = new NetPresentValueResultDto { DiscountRate = discountRate.Value, NetPresentValue = npv };
            results.Add(resultDto);
        }
        
        return results;
    }
}