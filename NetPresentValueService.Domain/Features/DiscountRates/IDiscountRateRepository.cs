namespace NetPresentValueService.Domain.Features.DiscountRates;

public interface IDiscountRateRepository
{
    Task<IncrementedDiscountRateDetails> GetAsync(string userId);
    Task SaveAsync(string userId, IncrementedDiscountRateDetails details);
}