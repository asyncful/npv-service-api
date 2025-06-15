using NetPresentValueService.Domain.Features.DiscountRates;

namespace NetPresentValueService.Application.Features.DiscountRates;

public static class IncrementedDiscountRateDetailsMapper
{
    public static IncrementedDiscountRateDetails ToDomainModel(
        this IncrementedDiscountRateDetailsDto dto)
    {
        var lowerBoundRate = new DiscountRate(dto.LowerBoundDiscountRate);
        var upperBoundRate = new DiscountRate(dto.UpperBoundDiscountRate);
        var increment = new DiscountRate(dto.Increment);
        return new IncrementedDiscountRateDetails(lowerBoundRate, upperBoundRate, increment);
    }
}