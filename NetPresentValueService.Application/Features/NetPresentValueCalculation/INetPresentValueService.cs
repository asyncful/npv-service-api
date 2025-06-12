namespace NetPresentValueService.Application.Features.NetPresentValueCalculation;

public interface INetPresentValueService
{
    Task<IEnumerable<NetPresentValueResultDto>> CalculateRangeAsync(string userId, List<decimal> cashFlows);
}