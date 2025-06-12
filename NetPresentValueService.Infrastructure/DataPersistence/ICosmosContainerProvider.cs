using Microsoft.Azure.Cosmos;

namespace NetPresentValueService.Infrastructure.DataPersistence;

public interface ICosmosContainerProvider
{
    Container GetContainer(string databaseName, string containerName);
}
