namespace NetPresentValueService.Application.Features.DiscountRates;

public class IncrementedDiscountRateDetailsDto
{
    public decimal LowerBoundDiscountRate { get; set; }
    public decimal UpperBoundDiscountRate { get; set; }
    public decimal Increment { get; set; }
}