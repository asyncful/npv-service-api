using NetPresentValueService.Application.Features.DiscountRates;
using NetPresentValueService.Domain.Features.DiscountRates;
using NetPresentValueService.Domain.Features.NetPresentValueCalculation;

namespace NetPresentValueService.Application.Features.NetPresentValueCalculation;

public class NetPresentValueResultSetDto
{
    public IReadOnlyList<NetPresentValueResultDto> Results { get; set; }
}