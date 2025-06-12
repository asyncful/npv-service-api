using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetPresentValueService.Application.Features.DiscountRates;
using NetPresentValueService.Application.Features.NetPresentValueCalculation;
using NetPresentValueService.Domain.Features.DiscountRates;
using NetPresentValueService.Infrastructure.DataPersistence;
using NetPresentValueService.Infrastructure.Features.DiscountRates;
using NpvMicroservice.Infrastructure.Persistence;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;

        services.AddSingleton<ICosmosContainerProvider, CosmosContainerProvider>();

        services.AddSingleton(sp =>
        {
            var connectionString = config["CosmosDb:ConnectionString"];
            return new Microsoft.Azure.Cosmos.CosmosClient(connectionString);
        });

        services.AddScoped<IDiscountRateRepository>(sp =>
        {
            var provider = sp.GetRequiredService<ICosmosContainerProvider>();
            var dbName = config["CosmosDb:Database"];
            var containerName = config["CosmosDb:DiscountDetailsContainer"];
            return new DiscountRateRepository(provider, dbName, containerName);
        });

        services.AddScoped<INetPresentValueService, NetPresentValueService.Application.Features.NetPresentValueCalculation.NetPresentValueService>();
        services.AddScoped<IDiscountRateService, DiscountRateService>();
    })
    .Build();

host.Run();