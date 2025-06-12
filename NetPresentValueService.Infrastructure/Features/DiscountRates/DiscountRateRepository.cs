using System.Net;
using Microsoft.Azure.Cosmos;
using NetPresentValueService.Domain.Features.DiscountRates;
using NetPresentValueService.Infrastructure.DataPersistence;

namespace NetPresentValueService.Infrastructure.Features.DiscountRates;

public class DiscountRateRepository : IDiscountRateRepository
{
    private readonly Container _container;

    public DiscountRateRepository(ICosmosContainerProvider provider, string databaseName, string containerName)
    {
        _container = provider.GetContainer(databaseName, containerName);
    }

    public async Task<IncrementedDiscountRateDetails> GetAsync(string userId)
    {
        try
        {
            var response = await _container.ReadItemAsync<IncrementedDiscountRateDetailsDocument>(userId, new PartitionKey(userId));
            var entity = response.Resource;
            return new IncrementedDiscountRateDetails(
                new DiscountRate(entity.LowerBound),
                new DiscountRate(entity.UpperBound),
                new DiscountRate(entity.Increment)
            );
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new InvalidOperationException("No discount settings configured.");
        }
    }

    public async Task SaveAsync(string userId, IncrementedDiscountRateDetails details)
    {
        var doc = new IncrementedDiscountRateDetailsDocument
        {
            Id = userId,
            PartitionKey = userId,
            LowerBound = details.LowerBoundDiscountRate.Value,
            UpperBound = details.UpperBoundDiscountRate.Value,
            Increment = details.Increment.Value
        };

        await _container.UpsertItemAsync(doc, new PartitionKey(userId));
    }
}