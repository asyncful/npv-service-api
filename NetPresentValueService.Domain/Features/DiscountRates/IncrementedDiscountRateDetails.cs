using NetPresentValueService.Domain.Exceptions;

namespace NetPresentValueService.Domain.Features.DiscountRates;

public class IncrementedDiscountRateDetails
{
    public DiscountRate LowerBoundDiscountRate { get; }
    public DiscountRate UpperBoundDiscountRate { get; }
    public DiscountRate Increment { get; }
    
    public IncrementedDiscountRateDetails(DiscountRate lowerBoundDiscountRate, DiscountRate upperBoundDiscountRate, DiscountRate increment)
    {
        if (upperBoundDiscountRate.Value < lowerBoundDiscountRate.Value)
            throw new DomainValidationException("Upper bound discount rate must be greater than or equal to lower bound discount rate.");
        if (increment.Value <= 0)
            throw new DomainValidationException("Increment must be positive.");
        if (increment.Value * 100 < upperBoundDiscountRate.Value - lowerBoundDiscountRate.Value)
            throw new DomainValidationException("Upper and Lower bound discount rates should not allow for more than 100 incremented discount rates.");

        LowerBoundDiscountRate = lowerBoundDiscountRate;
        UpperBoundDiscountRate = upperBoundDiscountRate;
        Increment = increment;
    }
}