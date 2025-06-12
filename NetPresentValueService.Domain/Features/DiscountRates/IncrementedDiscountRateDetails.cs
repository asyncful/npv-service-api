namespace NetPresentValueService.Domain.Features.DiscountRates;

public class IncrementedDiscountRateDetails
{
    public DiscountRate LowerBoundDiscountRate { get; }
    public DiscountRate UpperBoundDiscountRate { get; }
    public DiscountRate Increment { get; }
    
    public IncrementedDiscountRateDetails(DiscountRate lowerBoundDiscountRate, DiscountRate upperBoundDiscountRate, DiscountRate increment)
    {
        if (upperBoundDiscountRate.Value < lowerBoundDiscountRate.Value)
            throw new ArgumentException("Upper bound discount rate must be greater than or equal to lower bound discount rate.");
        if (increment.Value <= 0)
            throw new ArgumentException("Increment must be positive.");

        LowerBoundDiscountRate = lowerBoundDiscountRate;
        UpperBoundDiscountRate = upperBoundDiscountRate;
        Increment = increment;
    }
}