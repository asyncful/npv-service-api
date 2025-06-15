using FluentAssertions;
using NetPresentValueService.Domain.Exceptions;
using NetPresentValueService.Domain.Features.CashFlows;

namespace NetPresentValueService.Domain.Test.Features.CashFlows;

public class CashFlowSetTests
{
    [Fact]
    public void CreateCashFlowSetWithNullValuesList()
    {
        //Act
        var act = () => new CashFlowSet(null);

        //Assert
        act.Should().Throw<DomainValidationException>().WithMessage("There should be at least one cash flow.");
    }
    
    [Fact]
    public void CreateCashFlowSetWithEmptyValuesList()
    {
        //Act
        var act = () => new CashFlowSet(new List<decimal>());

        //Assert
        act.Should().Throw<DomainValidationException>().WithMessage("There should be at least one cash flow.");
    }
    
    [Fact]
    public void CreateCashFlowSetWithOver50Values()
    {
        //Arrange
        var values = Enumerable.Range(0, 51).Select(x => (decimal)x).ToList();
        
        //Act
        var act = () => new CashFlowSet(values);

        //Assert
        act.Should().Throw<DomainValidationException>().WithMessage("There should be no more than 50 cash flows.");
    }
    
    [Fact]
    public void CreateValidCashFlowSet()
    {
        //Arrange
        List<decimal> values = [-1000, 100, 200];
        
        //Act
        var actual = new CashFlowSet(values);

        //Assert
        actual.Values.Should().BeEquivalentTo(values);
    }
}