using NetPresentValueService.Application.Features.DiscountRates;
using NetPresentValueService.Domain.Features.CashFlows;
using NetPresentValueService.Domain.Features.DiscountRates;
using NetPresentValueService.Domain.Features.NetPresentValueCalculation;

namespace NetPresentValueService.Application.Features.NetPresentValueCalculation;

public class NetPresentValueService : INetPresentValueService
{
    public async Task<NetPresentValueResultSetDto> CalculateRangeAsync(NetPresentValueRequestDto requestDto)
    {
        var cashFlowSet = new CashFlowSet(requestDto.CashFlows);
        var results = new List<NetPresentValueResultDto>();
        
        var incrementDetails = requestDto.DiscountRateDetails.ToDomainModel();
        
        var discountRates = IncrementedDiscountRateCalculator.Calculate(incrementDetails);
        
        foreach (var discountRate in discountRates)
        {
            var npvResult = NetPresentValueCalculator.Calculate(cashFlowSet, discountRate);
            var resultDto = new NetPresentValueResultDto()
            {
                DiscountRate = npvResult.DiscountRate.Value,
                NetPresentValue = npvResult.NetPresentValue,
            };
            results.Add(resultDto);
        }
        
        return new NetPresentValueResultSetDto
        {
            Results = results
        };
    }
}