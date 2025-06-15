using NetPresentValueService.Domain.Exceptions;

namespace NetPresentValueService.Domain.Features.CashFlows;

public class CashFlowSet
{
    public IReadOnlyList<decimal> Values { get;}

    public CashFlowSet(IList<decimal> values)
    {
        if (values == null || values.Count == 0)
        {
            throw new DomainValidationException("There should be at least one cash flow.");
        }
        if (values.Count > 50)
        {
            throw new DomainValidationException("There should be no more than 50 cash flows.");

        }
        
        Values = values.ToList();
    }
}