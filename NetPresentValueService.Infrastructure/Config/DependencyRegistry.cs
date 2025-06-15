using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetPresentValueService.Application.Features.DiscountRates;
using NetPresentValueService.Application.Features.NetPresentValueCalculation;
using NetPresentValueService.Domain.Features.CashFlows;
using NetPresentValueService.Domain.Features.DiscountRates;

namespace NetPresentValueService.Infrastructure.Config;
    
public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<INetPresentValueService, Application.Features.NetPresentValueCalculation.NetPresentValueService>();

        return services;
    }
}

