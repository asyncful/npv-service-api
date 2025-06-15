namespace NetPresentValueService.Application.Features.NetPresentValueCalculation;

public interface INetPresentValueService
{
    Task<NetPresentValueResultSetDto> CalculateRangeAsync(NetPresentValueRequestDto requestDto);
}