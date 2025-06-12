namespace NetPresentValueService.Infrastructure.Features.DiscountRates;

public class IncrementedDiscountRateDetailsDocument
{
    public string Id { get; set; }
    public string PartitionKey { get; set; }
    public decimal LowerBound { get; set; }
    public decimal UpperBound { get; set; }
    public decimal Increment { get; set; }
}