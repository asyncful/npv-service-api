namespace NetPresentValueService.Application.Features.DiscountRates;

public interface IDiscountRateService
{
    Task SaveIncrementedDiscountRateDetails(string userId, IncrementedNetPresentValueDetailsDto detailsDto);
    Task<IncrementedNetPresentValueDetailsDto> LoadIncrementedDiscountRateDetails(string userId);
}