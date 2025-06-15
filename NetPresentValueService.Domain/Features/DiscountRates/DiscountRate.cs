using NetPresentValueService.Domain.Exceptions;

namespace NetPresentValueService.Domain.Features.DiscountRates;

public record DiscountRate
{
    public decimal Value { get; init; }

    public DiscountRate(decimal value)
    {
        if (value is < -1m or > 1m)
            throw new DomainValidationException("A discount rate should be between -1.0 and 1.0.");
        
        Value = value;
    }
}
