using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetPresentValueService.Application.Features.NetPresentValueCalculation;
using NetPresentValueService.Infrastructure.Config;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactDev", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // or "*" for any origin (not recommended for prod)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.RegisterServices();

var app = builder.Build();

app.Run();