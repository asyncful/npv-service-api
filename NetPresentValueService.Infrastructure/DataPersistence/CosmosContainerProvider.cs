using Microsoft.Azure.Cosmos;
using NetPresentValueService.Infrastructure.DataPersistence;

namespace NpvMicroservice.Infrastructure.Persistence
{
    public class CosmosContainerProvider : ICosmosContainerProvider
    {
        private readonly CosmosClient _cosmosClient;

        public CosmosContainerProvider(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        public Container GetContainer(string databaseName, string containerName)
        {
            return _cosmosClient.GetContainer(databaseName, containerName);
        }
    }
}