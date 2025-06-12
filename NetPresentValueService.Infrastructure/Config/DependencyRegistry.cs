using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetPresentValueService.Application.Features.DiscountRates;
using NetPresentValueService.Application.Features.NetPresentValueCalculation;
using NetPresentValueService.Domain.Features.DiscountRates;
using NetPresentValueService.Infrastructure.DataPersistence;
using NetPresentValueService.Infrastructure.Features.DiscountRates;
using NpvMicroservice.Infrastructure.Persistence;

namespace NetPresentValueService.Infrastructure.Config;


    
public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register CosmosClient as singleton
        services.AddSingleton(_ =>
        {
            var connectionString = configuration["CosmosDb:ConnectionString"];
            return new CosmosClient(connectionString);
        });

        // Register container provider
        services.AddSingleton<ICosmosContainerProvider, CosmosContainerProvider>();

        // Register repository
        services.AddScoped<IDiscountRateRepository>(sp =>
        {
            var provider = sp.GetRequiredService<ICosmosContainerProvider>();
            var dbName = configuration["CosmosDb:Database"];
            var containerName = configuration["CosmosDb:DiscountSettingsContainer"];
            return new DiscountRateRepository(provider, dbName, containerName);
        });

        return services;
    }

    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<INetPresentValueService, Application.Features.NetPresentValueCalculation.NetPresentValueService>();
        services.AddScoped<IDiscountRateService, DiscountRateService>();

        return services;
    }
}

