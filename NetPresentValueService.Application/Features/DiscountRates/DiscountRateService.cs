using NetPresentValueService.Domain.Features.DiscountRates;

namespace NetPresentValueService.Application.Features.DiscountRates;

public class DiscountRateService : IDiscountRateService
{
    private readonly IDiscountRateRepository _discountRateRepository;

    public DiscountRateService(IDiscountRateRepository discountRateRepository)
    {
        _discountRateRepository = discountRateRepository;
    }

    public async Task SaveIncrementedDiscountRateDetails(string userId, IncrementedNetPresentValueDetailsDto dto)
    {
        var details = new IncrementedDiscountRateDetails(
            new DiscountRate(dto.LowerBoundDiscountRate),
            new DiscountRate(dto.UpperBoundDiscountRate),
            new DiscountRate(dto.Increment)
        );

        await _discountRateRepository.SaveAsync(userId, details);
    }

    public async Task<IncrementedNetPresentValueDetailsDto> LoadIncrementedDiscountRateDetails(string userId)
    {
        var details = await _discountRateRepository.GetAsync(userId);
        return new IncrementedNetPresentValueDetailsDto
        {
            LowerBoundDiscountRate = details.LowerBoundDiscountRate.Value,
            UpperBoundDiscountRate = details.UpperBoundDiscountRate.Value,
            Increment = details.Increment.Value
        };
    }

}